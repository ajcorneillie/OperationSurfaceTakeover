using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    GameObject theBase;



    public GameObject collidedObject;

    int health = 12;
    int damage = 5;
    bool isAttacking = false;
    float moveSpeed = 0.5f;
    float attackSpeed = 2f;

    Timer enemyAttackTimer;
    
    GameEvent enemyAttack = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAttackTimer = gameObject.AddComponent<Timer>();
        enemyAttackTimer.Duration = attackSpeed;
        enemyAttackTimer.Run();
        EventManager.AddInvoker(GameplayEvent.EnemyAttack, enemyAttack);
        EventManager.AddListener(GameplayEvent.DamageEnemy, Damage);
        EventManager.AddListener(GameplayEvent.StructureDestroyed, StructureDestroyed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, theBase.transform.position, moveSpeed * Time.deltaTime);
            Vector2 direction = (theBase.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
        {
            isAttacking = true;
            collidedObject = collision.gameObject;
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
}
