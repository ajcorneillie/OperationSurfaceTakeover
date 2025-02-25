using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    float speed;
    int damage;
    private GameObject myTarget;
    private bool hasTarget = false;
    private Vector2 direction;
    GameObject Projectile;

    public bool isActive = false;

    GameEvent damageEnemy = new GameEvent();
    GameEvent bulletSpawned = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        EventManager.AddListener(GameplayEvent.BulletFired, SetTarget);
    }
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.DamageEnemy, damageEnemy);


        bulletSpawned.AddData(GameplayEventData.Bullet, Projectile);
        bulletSpawned.Invoke(bulletSpawned.Data);

    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget == true) 
        { 
            if (myTarget != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, myTarget.transform.position, speed * Time.deltaTime);
                
            }
            if (myTarget == null)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject collidedObject = collision.gameObject;
            damageEnemy.AddData(GameplayEventData.Damage, damage);
            damageEnemy.AddData(GameplayEventData.Enemy, collidedObject);
            damageEnemy.Invoke(damageEnemy.Data);

            Destroy(gameObject);
        }
    }

    public void SetTarget(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject target = (GameObject)output;

        data.TryGetValue(GameplayEventData.Bullet, out output);
        GameObject bullet = (GameObject)output;

        if (bullet == gameObject)
        {
            if (target != null)
            {
                myTarget = target;
                hasTarget = true;
            }
            if (target == null)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(int Damage, float ProjectileSpeed, GameObject projectile)
    {
        damage = Damage;
        speed = ProjectileSpeed;
        Projectile = projectile;
    }
}
