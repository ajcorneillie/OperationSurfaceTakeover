using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    float speed = 5f;
    int damage = 5;
    public GameObject myTarget;
    public GameObject myself;
    public bool hasTarget = false;
    public Vector2 direction;

    public bool isActive = false;

    GameEvent damageEnemy = new GameEvent();
    GameEvent bulletSpawned = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.BulletSpawned, bulletSpawned);
        EventManager.AddInvoker(GameplayEvent.DamageEnemy, damageEnemy);
        EventManager.AddListener(GameplayEvent.BulletFired, SetTarget);

        bulletSpawned.AddData(GameplayEventData.Bullet, myself);
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

        if (myself == bullet)
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
}
