using UnityEngine;

public class Cone : MonoBehaviour
{
    int myDamage;

    Timer destroyTimer;

    [SerializeField]
    Collider2D areaCollider;

    GameEvent damageEnemy = new GameEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        EventManager.AddInvoker(GameplayEvent.DamageEnemy, damageEnemy);
        destroyTimer = gameObject.AddComponent<Timer>();
        destroyTimer.Duration = 1;
        destroyTimer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (destroyTimer.Finished)
        {
            Collider2D[] objectsInside = Physics2D.OverlapBoxAll(areaCollider.bounds.center, areaCollider.bounds.extents, 0);

            foreach (Collider2D obj in objectsInside)
            {
                if (obj.CompareTag("Enemy"))
                {
                    GameObject collidedObject = obj.gameObject;
                    damageEnemy.AddData(GameplayEventData.Damage, myDamage);
                    damageEnemy.AddData(GameplayEventData.Enemy, collidedObject);
                    damageEnemy.Invoke(damageEnemy.Data);
                }
            }
            Destroy(gameObject);
        }
    }



    public void Initialize(int damage)
    {
        myDamage = damage;
    }
}
