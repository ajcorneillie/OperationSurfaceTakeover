using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Fields

    [SerializeField]
    AudioClip attackAudio; //reference to the attack audio of the enemy

    AudioSource myAudioSource; //reference to the audio source

    GameObject theBase;//reference to the base the enemy will seek out

    public GameObject collidedObject;//reference to the object the enemy collideds with

    //reference to the nodes on the first path the enemies can follow
    public GameObject node1;
    public GameObject node2;
    public GameObject node3;
    public GameObject node4;
    public GameObject node5;

    //reference to the nodes on the second path the enemies can follow
    public GameObject newnode1;
    public GameObject newnode2;
    public GameObject newnode3;
    public GameObject newnode4;
    public GameObject newnode5;

    public GameObject rallyPoint; //reference to the waiting point for the enemies

    int difficulty = 1; //sets the default behaviour of the enemy to 1

    GameObject spawner; //reference to the spawner that spawned this enemy

    //reference to stats found in the enemy scriptable object
    int health;
    int damage;
    float moveSpeed;
    float attackSpeed = 2f;
    float size;

    bool isAttacking = false; //boolean to decide if the enmey is currently attacking a structure
   
    float moveSpeed2; //holds a temporary reference to the movement speed to remove movement after attacking

    int nodeIndex = 0; //holds the index of the current node the enemy is heading to

    int wave; //determines what wave it is to determine enemy behaviour

    int ralliesHit = 0; //decides if a rally has been hit or not

    Timer enemyAttackTimer; //timer to determine the enemy attack speed
    
    //game events to be invoked
    GameEvent enemyAttack = new GameEvent();
    GameEvent waitingRally = new GameEvent();
    GameEvent enemySpawn = new GameEvent();
    GameEvent enemyDeath = new GameEvent();

    List <GameObject> nodes = new List <GameObject>(); //list of nodes for the first path the enemies can follow
    List<GameObject> newnodes = new List<GameObject>(); //list of nodes for the second path the enemies can follow

    bool atRally = false; //determines if at the rally point

    bool rallyActive = false; // determines if the current rally has been set to active
    #endregion

    #region Unity Methods

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //reference to this game object's audio source

        //timer support for the enemy attack speed timer
        enemyAttackTimer = gameObject.AddComponent<Timer>();
        enemyAttackTimer.Duration = attackSpeed;
        enemyAttackTimer.Run();

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.EnemyAttack, enemyAttack);
        EventManager.AddInvoker(GameplayEvent.EnemySpawn, enemySpawn);
        EventManager.AddInvoker(GameplayEvent.EnemyDeath, enemyDeath);
        EventManager.AddInvoker(GameplayEvent.WaitingRally, waitingRally);

        //events this script listens to
        EventManager.AddListener(GameplayEvent.Wave, WaveUpdate);
        EventManager.AddListener(GameplayEvent.DamageEnemy, Damage);
        EventManager.AddListener(GameplayEvent.RallyActivate, RallyActivate);
        EventManager.AddListener(GameplayEvent.StructureDestroyed, StructureDestroyed);
        EventManager.AddListener(GameplayEvent.NodeHit, NodeHit);

        //sets the nodes of the first path the enemies can follow into the first path list
        node1 = spawner.GetComponent<Level1Spawner>().node1;
        node2 = spawner.GetComponent<Level1Spawner>().node2;
        node3 = spawner.GetComponent<Level1Spawner>().node3;
        node4 = spawner.GetComponent<Level1Spawner>().node4;
        node5 = spawner.GetComponent<Level1Spawner>().node5;

        //sets the nodes of the second path the enemies can follow into the second path list
        newnode1 = spawner.GetComponent<Level1Spawner>().newnode1;
        newnode2 = spawner.GetComponent<Level1Spawner>().newnode2;
        newnode3 = spawner.GetComponent<Level1Spawner>().newnode3;
        newnode4 = spawner.GetComponent<Level1Spawner>().newnode4;
        newnode5 = spawner.GetComponent<Level1Spawner>().newnode5;

        rallyPoint = spawner.GetComponent<Level1Spawner>().rallyPoint; //defines this enemy's rally point

        //adds the nodes of the first path into the first path list in order
        nodes.Add(node1);
        nodes.Add(node2);
        nodes.Add(node3);
        nodes.Add(node4);
        nodes.Add(node5);

        //adds the nodes of the second path into the second path list in order
        newnodes.Add(newnode1);
        newnodes.Add(newnode2);
        newnodes.Add(newnode3);
        newnodes.Add(newnode4);
        newnodes.Add(newnode5);

        //invokes the enemy spawn event while passing in this game object for data
        enemySpawn.AddData(GameplayEventData.Enemy, gameObject);
        enemySpawn.Invoke(enemySpawn.Data);
    }

    // Update is called once per frame
    void Update()
    {
        //determines if the enemy is of type 1 and not currently attacking
        if (isAttacking == false && theBase != null && difficulty == 1)
        {
            //if there are more nodes in the first path list then the one the enemy is currently on go inside
            if (nodeIndex < nodes.Count)
            {
                //moves the enemy towards the next node in the list
                transform.position = Vector3.MoveTowards(transform.position, nodes[nodeIndex].transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (nodes[nodeIndex].transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                //if there are no more nodes in the first path list move towards the base
                transform.position = Vector3.MoveTowards(transform.position, theBase.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (theBase.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            
        }

        //determines if the enemy is of type 2 and not currently attacking
        if (isAttacking == false && theBase != null && difficulty == 2)
        {
            //if there are more nodes in the second path list then the one the enemy is currently on go inside
            if (nodeIndex < newnodes.Count)
            {
                //moves the enemy towards the next node in the list
                transform.position = Vector3.MoveTowards(transform.position, newnodes[nodeIndex].transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (newnodes[nodeIndex].transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                //if there are no more nodes in the second path list move towards the base
                transform.position = Vector3.MoveTowards(transform.position, theBase.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (theBase.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

        }

        //determines if the enemy is of type 3 and not currently attacking
        if (isAttacking == false && theBase != null && difficulty == 3)
        {
            //checks if the enemy is already at the rally
            if (atRally == false)
            {
                //if the enemy is not at the rally move towards the rally
                transform.position = Vector3.MoveTowards(transform.position, rallyPoint.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (rallyPoint.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            //if the rally is active set speed back to normal and pick a random path of 1 or 2 to follow
            if (rallyActive == true)
            {
                moveSpeed = moveSpeed2;
                difficulty = Random.Range(1, 3);
            }
        }

        //if currently attacking is true and the attack speed timer is finished go inside
        if (isAttacking == true && enemyAttackTimer.Finished)
        {

            //checks if the currently collided object exsists
            if(collidedObject != null)
            {
                //it true plays the audio sound far attacking
                myAudioSource.PlayOneShot(attackAudio);

                //invokes the enemy attack event passing in the collided structure, damage amount, and this current game object
                enemyAttack.AddData(GameplayEventData.Structure, collidedObject);
                enemyAttack.AddData(GameplayEventData.Damage, damage);
                enemyAttack.AddData(GameplayEventData.Enemy, gameObject);
                enemyAttack.Invoke(enemyAttack.Data);

                //resets the enemy attack timer
                enemyAttackTimer.Run();
            }
        }

    }

    /// <summary>
    /// detects for collisions with this game object's collision hitbox
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if the collision was the structure of the base
        if (collision.gameObject.CompareTag("Structure") || collision.gameObject.CompareTag("Base"))
        {
            isAttacking = true; //sets the is attacking state to true

            collidedObject = collision.gameObject; //sets the collided enemy game object to that of the collision
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the event of a new node being hit by an enemy
    /// </summary>
    /// <param name="data"></param>
    private void NodeHit(Dictionary<System.Enum, object> data)
    {
        //tries to collect the data on the enemy in the collision
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        //tries to collect the node in the collision
        data.TryGetValue(GameplayEventData.Node, out output);
        GameObject node = (GameObject)output;

        //checks if the enemy behaviour type is 1
        if (difficulty == 1)
        {
            //checks if the current index is less than the node given in the event
            if (nodeIndex < nodes.Count)
            {
                //checks if the given node and enemy in the event are the same as this game object and the current node the enemy is at on the first path
                if (gameObject == enemy && node == nodes[nodeIndex])
                {
                    //increases to the next node
                    nodeIndex++;
                }
            }
        }

        //checks if the enemy behaviour typs is 2
        if (difficulty == 2)
        {
            //checks if the current index is less than the node given in the event
            if (nodeIndex < newnodes.Count)
            {
                //checks if the given node and enemy in the event are the same as this game object and the current node the enemy is at on the second path
                if (gameObject == enemy && node == newnodes[nodeIndex])
                {
                    //increases to the next node
                    nodeIndex++;
                }
            }
        }

        //checks if the enemy behaviour is type 3, has not yet hit a rally point, and if the rally point is the same as the node in the event
        if (difficulty == 3 && enemy == gameObject && node == rallyPoint && ralliesHit < 1)
        {
            ralliesHit++;//increases the number of rallies hit by 1

            waitingRally.Invoke(waitingRally.Data);//Invokes the waiting at rally event

            atRally = true;//sets the at rally state to true
        }


    }
    
    /// <summary>
    /// listens for the event of the enemy taking damage
    /// </summary>
    /// <param name="data"></param>
    void Damage(Dictionary<System.Enum, object> data)
    {
        //tries to get the enemy that is suppossed to take damage
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        //tries to get the damage amount that is suppossed to be applied
        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

        //determines if the enemy passed in the event is the same as this game object
        if (gameObject == enemy)
        {
            //lowers health the amount of the damage passed in the event
            health = health - damage;

            // if health is less than or equal to 0
            if (health <= 0)
            {
                //invokes the enemy dying event while passing in this enem's reference
                enemyDeath.AddData(GameplayEventData.Enemy, gameObject);
                enemyDeath.Invoke(enemyDeath.Data);

                Destroy(gameObject); //destroys the current game object
            }
        }

    }

    /// <summary>
    /// listens for the event of a structure being destroyed
    /// </summary>
    /// <param name="data"></param>
    void StructureDestroyed(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the structure that was destroyed
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        //if the structure destroyed is the same as the one this object was in collision with
        if (collidedObject = structure.gameObject)
        {
            isAttacking = false; //sets the is attacking state to false
        }
    }

    /// <summary>
    /// initializes the enemy and it's paramaters when spawned
    /// </summary>
    /// <param name="thatBase"></param>
    /// <param name="scriptable"></param>
    /// <param name="Spawner"></param>
    /// <param name="Wave"></param>
    public void Initialize(GameObject thatBase, EnemyScriptable scriptable, GameObject Spawner, int Wave)
    {
        theBase = thatBase; //the base the enemy will seek out

        //stats of the enemy gathered from the scriptable object being passed through
        health = scriptable.Health;
        moveSpeed = scriptable.MoveSpeed;
        damage = scriptable.Damage;
        difficulty = scriptable.difficulty;
        size = scriptable.size;
        attackSpeed = scriptable.attackSpeed;

        spawner = Spawner; //sets the spawner this enemy will get its nodes from

        transform.localScale = new Vector3(size, size, 0); // sets the scale of the enemy to its scriptable object's size

        wave = Wave; //sets the wave to that of the current in game wave

        moveSpeed2 = moveSpeed; //sets a constant to be used to change move speed to later

        //if the current wave is greater than 20
        if (wave + 1 > 20)
        {
            //if the enemy behaviour type is 1
            if (difficulty == 1)
            {
                //sets the enemy behaviour at random to 1 or 2 to detrmine enmy pathing options
                int pickPath = Random.Range(0, 2);
                if (pickPath == 0)
                {
                    difficulty = 1;
                }
                if (pickPath == 1)
                {
                    difficulty = 2;
                }
            }
        }

        // if the wave is less than 20 and the enemy behaviour type is 3
        if (wave + 1 < 20 && difficulty == 3)
        {
            difficulty = 1; //sets the enemy behaviour type to 1
        }
    }

    /// <summary>
    /// listens for the event of the rally activateing
    /// </summary>
    /// <param name="data"></param>
    private void RallyActivate(Dictionary<System.Enum, object> data)
    {
        rallyActive = true; //sets the rally active state to true
    }

    /// <summary>
    /// listens for the when a new wave starts
    /// </summary>
    /// <param name="data"></param>
    void WaveUpdate(Dictionary<System.Enum, object> data)
    {
        //tries to get the data on the current wave
        data.TryGetValue(GameplayEventData.Wave, out object output);
        int wave = (int)output;

        //trie to get the data on the max wave of the level
        data.TryGetValue(GameplayEventData.MaxWave, out output);
        int maxWave = (int)output;

        //if the current wave is the final wave
        if (wave == maxWave)
        {
            rallyActive = true; //sets the rally active state to true
        }
    }
    #endregion
}
