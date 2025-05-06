using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    #region Fields
    float speed; //sets the speed of the bullet

    int damage; //sets the damage of the bullet

    private GameObject myTarget; //reference to the target the bullet will seek out

    private bool hasTarget = false; //boolean for if the bullet has a target of not

    private Vector3 direction; //reference to the direction the bullet will move in

    GameObject Projectile; //reference to the type of projectile

    public bool isActive = false; //sets default to inactive

    Timer lifeTimer; //reference to the timer for the life of a projectile

    //support for events this script invokes
    GameEvent damageEnemy = new GameEvent();
    GameEvent bulletSpawned = new GameEvent();
    #endregion

    #region Unity Methods
    // awake is called before the first frame
    private void Awake()
    {
        EventManager.AddListener(GameplayEvent.BulletFired, SetTarget); //events this script listens for
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.DamageEnemy, damageEnemy); //events this script invokes
        
        //invokes the spawn bullet event while passing in this game object as data
        bulletSpawned.AddData(GameplayEventData.Bullet, Projectile);
        bulletSpawned.Invoke(bulletSpawned.Data);
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the life timer has finished
        if (lifeTimer.Finished)
        {
            Destroy(gameObject); //destroys this game object
        }
        
    }

    // Fixed update is called once every frame
    private void FixedUpdate()
    {
        //checks if this object has a target
        if (hasTarget == true)
        {
            GetComponent<Rigidbody2D>().linearVelocity  = direction * speed; //sets the speed and direction of the rigid body on this object
        }
    }

    //checks for if an object enters the trigger collision of this object
    void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the collision object has the enemy tag
        if (collision.CompareTag("Enemy"))
        {
            GameObject collidedObject = collision.gameObject; //sets the collided object to the object that was collided with

            //invokes the damage enemy event while passing in the damage and enemy it collided with as data
            damageEnemy.AddData(GameplayEventData.Damage, damage);
            damageEnemy.AddData(GameplayEventData.Enemy, collidedObject);
            damageEnemy.Invoke(damageEnemy.Data);

            Destroy(gameObject); //destroys self
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the set target event
    /// </summary>
    /// <param name="data"></param>
    public void SetTarget(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the enemy to target
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject target = (GameObject)output;

        //tries to get the data for the bullet that will target
        data.TryGetValue(GameplayEventData.Bullet, out output);
        GameObject bullet = (GameObject)output;

        //checks if the bullet setting a target is this game object
        if (bullet == gameObject)
        {
            //checks if the target is null
            if (target != null)
            {
                myTarget = target; //sets the target to the target passed in the event

                direction = (myTarget.transform.position - transform.position).normalized;// sets direction to that of the target

                hasTarget = true; //sets has target to true

                //timer support for life time of the bullet and runs the timer
                lifeTimer = gameObject.AddComponent<Timer>();
                lifeTimer.Duration = 0.5f;
                lifeTimer.Run();
            }
        }
    }

    /// <summary>
    /// initializes the base paramaters of the bullet
    /// </summary>
    /// <param name="Damage"></param>
    /// <param name="ProjectileSpeed"></param>
    /// <param name="projectile"></param>
    public void Initialize(int Damage, float ProjectileSpeed, GameObject projectile)
    {
        //sets the damage, speed, and projectile type of this projectile
        damage = Damage;
        speed = ProjectileSpeed;
        Projectile = projectile;
    }
    #endregion
}
