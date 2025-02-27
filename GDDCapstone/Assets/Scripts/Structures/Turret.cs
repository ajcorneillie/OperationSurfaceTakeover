using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Turret : MonoBehaviour
{
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


    private bool inRange = false;

    private GameObject enemy;
    private Vector3 direction;
    private float angle;
    private GameObject closest;
    private GameObject[] enemies;
    private float distance;

    GameObject myTile;

    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    private string enemyTag = "Enemy";

    private Timer fireSpeed;

    TurretButtonEnum turretButtonEnum;

    GameEvent bulletFired = new GameEvent();
    GameEvent destroyed = new GameEvent();
    GameEvent moneyUpdate = new GameEvent();
    /// <summary>
    /// Runs When this object is enabled
    /// </summary>
    private void OnEnable()
    {
        controls.Enable();
        place.Enable();
        cancelPlace.Enable();
    }

    /// <summary>
    /// Runs When this object is disabled
    /// </summary>
    private void OnDisable()
    {
        controls.Disable();
        place.Disable();
        cancelPlace.Disable();
    }

    private void Awake()
    {
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);
        EventManager.AddInvoker(GameplayEvent.BulletFired, bulletFired);
        EventManager.AddInvoker(GameplayEvent.StructureDestroyed, destroyed);
        EventManager.AddInvoker(GameplayEvent.GoldSpent, moneyUpdate);

        moneyUpdate.AddData(GameplayEventData.Gold, cost);
        moneyUpdate.Invoke(moneyUpdate.Data);

        fireSpeed = gameObject.AddComponent<Timer>();
        fireSpeed.Duration = atkSpeed;
        fireSpeed.Run();
    }

    private void Update()
    {

            enemy = FindClosestEnemy();
            if (enemy != null)
            {
                direction = enemy.transform.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                if (inRange == true && fireSpeed.Finished)
                {
                    if (turretButtonEnum == TurretButtonEnum.Turret || turretButtonEnum == TurretButtonEnum.MachineGunTurret)
                    {
                        GameObject thisProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                        thisProjectile.GetComponent<Bullet>().Initialize(damage, projectileSpeed, projectile);
                        bulletFired.AddData(GameplayEventData.Bullet, thisProjectile);
                        bulletFired.AddData(GameplayEventData.Enemy, enemy);
                        bulletFired.Invoke(bulletFired.Data);
                        fireSpeed.Run();
                    }

                    if (turretButtonEnum == TurretButtonEnum.FlameThrower)
                    {
                        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
                        
                        GameObject thisProjectile = Instantiate(projectile, transform.position, rotation);
                        thisProjectile.GetComponent<Cone>().Initialize(damage);
                        fireSpeed.Run();
                    }
                    
                }
            }

    }

    private GameObject FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        closest = null;
        maxRange = 7f;

        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);

            if (distance <= maxRange)
            {
                inRange = true;
                maxRange = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void EnemyAttack(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

        if (structure == gameObject)
        {
            health = health - damage;

            if (health <= 0)
            {
                destroyed.AddData(GameplayEventData.Structure, gameObject);
                destroyed.Invoke(destroyed.Data);
                myTile.GetComponent<Node>().ResetMe();
                Destroy(gameObject);
            }
        }


    }

    public void Initialize(StructureButton structureButton, GameObject tile)
    {
        health = structureButton.Health;
        atkSpeed = structureButton.AtkSpeed;
        maxRange = structureButton.MaxRange;
        minRange = structureButton.MinRange;
        damage = structureButton.Damage;
        cost = structureButton.Cost;
        structure = structureButton.Structure;
        tileSize = structureButton.TileSize;
        projectile = structureButton.Projectile;
        projectileSpeed = structureButton.ProjectileSpeed;
        myTile = tile;
        turretButtonEnum = structureButton.PurchaseButtonEnum;
        

        fireSpeed = gameObject.AddComponent<Timer>();
        fireSpeed.Duration = atkSpeed;
        fireSpeed.Run();
    }

}
