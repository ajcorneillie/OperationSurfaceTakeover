using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    GameObject theBase;

    GameObject structure;
    GameObject nearestStructure;
    private GameObject closest;
    private GameObject[] structures;
    private float distance;

    public GameObject collidedObject;

    public GameObject node1;
    public GameObject node2;
    public GameObject node3;
    public GameObject node4;
    public GameObject node5;

    public GameObject rallyPoint;
    int difficulty = 1;
    Sprite sprite;

    Camera cam;

    private string structureTag = "Structure";

    int health;
    int damage;
    bool isAttacking = false;
    float moveSpeed;
    float attackSpeed = 2f;
    float moveSpeed2;
    int nodeIndex = 0;

    bool isFlying = false;
    bool isTargetStructure = false;

    int attackType;

    Timer enemyAttackTimer;
    
    GameEvent enemyAttack = new GameEvent();
    GameEvent waitingRally = new GameEvent();
    GameEvent enemySpawn = new GameEvent();
    GameEvent enemyDeath = new GameEvent();

    List <GameObject> nodes = new List <GameObject>();

    bool atRally = false;
    bool rallyReady = false;
    bool rallyActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAttackTimer = gameObject.AddComponent<Timer>();
        enemyAttackTimer.Duration = attackSpeed;
        enemyAttackTimer.Run();
        EventManager.AddInvoker(GameplayEvent.EnemyAttack, enemyAttack);
        EventManager.AddInvoker(GameplayEvent.EnemySpawn, enemySpawn);
        EventManager.AddInvoker(GameplayEvent.EnemyDeath, enemyDeath);
        EventManager.AddInvoker(GameplayEvent.WaitingRally, waitingRally);
        EventManager.AddListener(GameplayEvent.DamageEnemy, Damage);
        EventManager.AddListener(GameplayEvent.RallyActivate, RallyActivate);
        EventManager.AddListener(GameplayEvent.StructureDestroyed, StructureDestroyed);
        EventManager.AddListener(GameplayEvent.NodeHit, NodeHit);

        moveSpeed2 = moveSpeed;

        Vector3 position = cam.GetComponent<EnemyPathingManager>().node1.transform.position;
        node1 = cam.GetComponent<EnemyPathingManager>().node1;
        node2 = cam.GetComponent<EnemyPathingManager>().node2;
        node3 = cam.GetComponent<EnemyPathingManager>().node3;
        node4 = cam.GetComponent<EnemyPathingManager>().node4;
        node5 = cam.GetComponent<EnemyPathingManager>().node5;

        rallyPoint = cam.GetComponent<EnemyPathingManager>().RallyPoint;

        enemySpawn.AddData(GameplayEventData.Enemy, gameObject);
        enemySpawn.Invoke(enemySpawn.Data);

    }

    // Update is called once per frame
    void Update()
    {


        if (isAttacking == false && theBase != null && difficulty == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, theBase.transform.position, moveSpeed * Time.deltaTime);
            Vector2 direction = (theBase.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (isAttacking == false && theBase != null && difficulty == 2)
        {
            structure = FindClosestStructure();
            transform.position = Vector3.MoveTowards(transform.position, structure.transform.position, moveSpeed * Time.deltaTime);
            Vector2 direction = (structure.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (isAttacking == false && theBase != null && difficulty == 3)
        {
            if (nodeIndex <= 4)
            {
                transform.position = Vector3.MoveTowards(transform.position, nodes[nodeIndex].transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (nodes[nodeIndex].transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            if (nodeIndex >= 5)
            {
                transform.position = Vector3.MoveTowards(transform.position, theBase.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (theBase.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

        }
        if (isAttacking == false && theBase != null && difficulty == 4)
        {
            if (atRally == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, rallyPoint.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (rallyPoint.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            if (atRally == true && rallyReady == false)
            {
                moveSpeed = 0;
                rallyReady = true;
                waitingRally.Invoke(waitingRally.Data);
            }
            if (rallyActive == true)
            {
                attackType = 0;
                moveSpeed = moveSpeed2;

            }
        }

        if (isAttacking == true && enemyAttackTimer.Finished)
        {
            if(collidedObject != null)
            {
                enemyAttack.AddData(GameplayEventData.Structure, collidedObject);
                enemyAttack.AddData(GameplayEventData.Damage, damage);
                enemyAttack.AddData(GameplayEventData.Enemy, gameObject);
                enemyAttack.Invoke(enemyAttack.Data);
                enemyAttackTimer.Run();
            }
        }

    }
    private GameObject FindClosestStructure()
    {
        structures = GameObject.FindGameObjectsWithTag(structureTag);
        closest = null;
        float maxRange = 1000;
        foreach (GameObject enemy in structures)
        {
            distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);

            if (distance <= maxRange)
            {
                maxRange = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void NodeHit(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        data.TryGetValue(GameplayEventData.Node, out output);
        GameObject node = (GameObject)output;
        if (nodeIndex < 5)
        {
            if (gameObject == enemy && node == nodes[nodeIndex])
            {
                if (attackType == 0 || attackType == 2)
                {
                    nodeIndex++;
                }
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
        {
            isAttacking = true;
            collidedObject = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Rally") && attackType == 3)
        {
            atRally = true;
        }
    }
    void Damage(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

            if (gameObject == enemy)
            {
                health = health - damage;

                if (health <= 0)
                {
                    enemyDeath.AddData(GameplayEventData.Enemy, gameObject);
                    enemyDeath.Invoke(enemyDeath.Data);
                    Destroy(gameObject);
                }
            }

    }

    void StructureDestroyed(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        if (collidedObject = structure.gameObject)
        {
            isAttacking = false;
        }
    }

    public void Initialize(GameObject thatBase, EnemyScriptable scriptable, Camera camera, GameObject rally, GameObject node1, GameObject node2, GameObject node3, GameObject node4, GameObject node5)
    {
        theBase = thatBase;

        health = scriptable.Health;
        moveSpeed = scriptable.MoveSpeed;
        damage = scriptable.Damage;
        isFlying = scriptable.isFlying;
        isTargetStructure = scriptable.TargetStructures;
        sprite = scriptable.mySprite;
        cam = camera;
        rallyPoint = rally;
        difficulty = scriptable.difficulty;
    }

    private void RallyActivate(Dictionary<System.Enum, object> data)
    {
        rallyActive = true;
    }
}
