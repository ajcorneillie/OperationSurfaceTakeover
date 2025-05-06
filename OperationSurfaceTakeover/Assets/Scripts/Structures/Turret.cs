using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Turret : MonoBehaviour
{
    #region Fields
    //references to audio clips for dying, turret fire, minigun fire, and purchasing
    [SerializeField]
    AudioClip dieAudio;
    [SerializeField]
    AudioClip turretAudio;
    [SerializeField]
    AudioClip minigunAudio;
    [SerializeField]
    AudioClip PurchaseAudio;

    AudioSource myAudioSource; //reference to this game object's audio source

    //references to this objects base stats for health, attack speed, range, damage, cost, object, icon, size, projectile it shoots, projectile speed, and starting max range
    public int health;
    public float atkSpeed;
    public float maxRange;
    public float minRange;
    public int damage;
    public int cost;
    public GameObject structure;
    public Sprite icon;
    public int tileSize;
    public GameObject projectile;
    public float projectileSpeed;
    public float startMaxRange;

    private bool inRange = false; //boolean for if an object is in range set to false

    private GameObject enemy; // reference to the enemy to target

    private Vector3 direction; //reference for where to rotate

    private float angle; //reference to the angle this object will face

    private GameObject closest; //reference to the closest enemy

    private GameObject[] enemies; //list of enemy objects

    private float distance; //reference to the distance away an enemy is

    GameObject myTile; //reference to the tile this object is on

    //support for input actions
    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    private string enemyTag = "Enemy"; //reference to the tag enemies have

    private Timer fireSpeed; //timer to seperate each shot of the turret

    TurretButtonEnum turretButtonEnum; //reference to the eneum to determine what turret this is

    //support for events this script invokes
    GameEvent bulletFired = new GameEvent();
    GameEvent destroyed = new GameEvent();
    GameEvent moneyUpdate = new GameEvent();
    #endregion

    #region Unity Methods
    // Runs When this object is enabled
    private void OnEnable()
    {
        //enables the input actions
        controls.Enable();
        place.Enable();
        cancelPlace.Enable();
    }

    // Runs When this object is disabled
    private void OnDisable()
    {
        //disables the input actions
        controls.Disable();
        place.Disable();
        cancelPlace.Disable();
    }

    //plays before the first frame
    private void Awake()
    {
        //support for the input actions
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to this object's audio source

        myAudioSource.PlayOneShot(PurchaseAudio, 1.5f); //plays the purchase audio clip at 150 percent audio

        //events this script listens for
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.BulletFired, bulletFired);
        EventManager.AddInvoker(GameplayEvent.StructureDestroyed, destroyed);
        EventManager.AddInvoker(GameplayEvent.GoldSpent, moneyUpdate);

        //invokes the money update event while passing in the cost of this turret as data
        moneyUpdate.AddData(GameplayEventData.Gold, cost);
        moneyUpdate.Invoke(moneyUpdate.Data);

        //support for the fire speed timer that seperates each shot
        fireSpeed = gameObject.AddComponent<Timer>();
        fireSpeed.Duration = atkSpeed;
        fireSpeed.Run();
    }

    //update is called once every frame
    private void Update()
    {
        enemy = FindClosestEnemy(); //runs the find closest enemy method and sets the enemy to the enemy that method finds

        //checks if the enemy exsists
        if (enemy != null)
        {
            //sets the direction to face and the distance betwwen the turret and the enemy
            direction = enemy.transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            //checks if the enemy is in range and the fire speed timer is finished
            if (inRange == true && fireSpeed.Finished)
            {
                //checks if this turret button enum is either the minigun or the base turret
                if (turretButtonEnum == TurretButtonEnum.Turret || turretButtonEnum == TurretButtonEnum.MachineGunTurret)
                {
                    //finds the x and y component of the vector 2 between the enemy and the turret, and then places the bullet 0.2 units further on that line than the turret firing it
                    float xComponent = direction.x;
                    float yComponent = direction.y;
                    xComponent = xComponent * 0.2f;
                    yComponent = yComponent * 0.2f;
                    Vector3 newPosition = transform.position;
                    newPosition.x = newPosition.x + xComponent;
                    newPosition.y = newPosition.y + yComponent;

                    GameObject thisProjectile = Instantiate(projectile, newPosition, Quaternion.identity); //spawns the bullet at the given position with no rotation

                    thisProjectile.GetComponent<Bullet>().Initialize(damage, projectileSpeed, projectile); //runs the initialize method of the bullet that was spawned

                    //invokes the bullet fired event and passes in the bullet and the enemy it is going towards as data
                    bulletFired.AddData(GameplayEventData.Bullet, thisProjectile);
                    bulletFired.AddData(GameplayEventData.Enemy, enemy);
                    bulletFired.Invoke(bulletFired.Data);

                    fireSpeed.Run(); //runs the bullet fired timer

                    //checks if the turret button enum is the base turret
                    if (turretButtonEnum == TurretButtonEnum.Turret)
                    {
                        myAudioSource.PlayOneShot(turretAudio, 0.35f); //plays the turret audio clip
                    }

                    //checks if the turret button enum is the minigun turret
                    if (turretButtonEnum == TurretButtonEnum.MachineGunTurret)
                    {
                        myAudioSource.PlayOneShot(minigunAudio, 0.35f); //plays the minigun turret audio clip
                    }

                }

                //checks if this turret button eneum is the flame thrower
                if (turretButtonEnum == TurretButtonEnum.FlameThrower)
                {
                    //finds the x and y component of the vector 2 between the enemy and the turret, and then places the flame cone 0.2 units further on that line than the turret firing it
                    Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
                    float xComponent = direction.x;
                    float yComponent = direction.y;
                    xComponent = xComponent * 0.2f;
                    yComponent = yComponent * 0.2f;
                    Vector3 newPosition = transform.position;
                    newPosition.x = newPosition.x + xComponent;
                    newPosition.y = newPosition.y + yComponent;

                    GameObject thisProjectile = Instantiate(projectile, newPosition, rotation); //spawns the flame cone at the given position with given rotation

                    thisProjectile.GetComponent<Cone>().Initialize(damage); //runs the initialize method on the flame cone that was just spawned

                    fireSpeed.Run(); //runs the fire speed timer
                }
                    
            }
        }

    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// method to find the closest enemy to this turret
    /// </summary>
    /// <returns></returns>
    private GameObject FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag(enemyTag); //finds all the enmies with the tag enemyTag and puts them into the enemies list

        closest = null; //sets the closest enemy to null

        maxRange = startMaxRange; // sets the max range

        //goes through all the enemies in the enemies list
        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position); //determines the distance between the enemy and the turret

            //checks if the distance between the enemy and the turret exceed the max distance
            if (distance <= maxRange)
            {
                inRange = true; //sets in range to true

                maxRange = distance; //sets the new max range to the distance to this enemy

                closest = enemy; //sets closest enemy to this enemy
            }
        }
        return closest; //returns the gamobject of the closest enemy
    }

    /// <summary>
    /// listens for the enemy attack event
    /// </summary>
    /// <param name="data"></param>
    private void EnemyAttack(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the structure attacked
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        //tries to get the data of the damage applied in the event
        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

        //checks if the structure passed in the event is the same as this game object
        if (structure == gameObject)
        {
            health = health - damage; //subtracts the damage passed in the event from the current health of this turret

            //checks if health is 0 or less
            if (health <= 0)
            {
                myAudioSource.PlayOneShot(dieAudio); //plays the turret death audio clip

                //invokes the destroyed event while passing in this game object as data
                destroyed.AddData(GameplayEventData.Structure, gameObject);
                destroyed.Invoke(destroyed.Data);

                myTile.GetComponent<Node>().ResetMe(); //runs the reset me method on the tile this turet was on

                Destroy(gameObject); // destroys self
            }
        }


    }

    /// <summary>
    /// method to initialize the stats of this turret
    /// </summary>
    /// <param name="structureButton"></param>
    /// <param name="tile"></param>
    public void Initialize(StructureButton structureButton, GameObject tile)
    {
        //gathered stats from the turretscriptable object
        health = structureButton.Health;
        atkSpeed = structureButton.AtkSpeed;
        startMaxRange = structureButton.MaxRange;
        maxRange = structureButton.MaxRange;
        minRange = structureButton.MinRange;
        damage = structureButton.Damage;
        cost = structureButton.Cost;
        structure = structureButton.Structure;
        tileSize = structureButton.TileSize;
        projectile = structureButton.Projectile;
        projectileSpeed = structureButton.ProjectileSpeed;
        turretButtonEnum = structureButton.PurchaseButtonEnum;

        myTile = tile; //sets the reference of the tile this turret is on

        //support for the fire speed timer and runs the timer
        fireSpeed = gameObject.AddComponent<Timer>();
        fireSpeed.Duration = atkSpeed;
        fireSpeed.Run();
    }
    #endregion
}
