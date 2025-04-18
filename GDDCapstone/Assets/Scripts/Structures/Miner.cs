using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class Miner : MonoBehaviour
{
    Timer miningTimer;
    bool isFull = false;
    bool isIdle = false;
    int mineSpeed = 3;
    float moveSpeed = 3f;
    int goldStorage = 100;

    [SerializeField]
    GameObject Base;

    [SerializeField]
    GameObject Mine;

    GameEvent goldDropoff = new GameEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.GoldDropoff, goldDropoff);
        miningTimer = gameObject.AddComponent<Timer>();
        miningTimer.Duration = mineSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle == false)
        {
            if (isFull == false && Mine != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, Mine.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (Mine.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (isFull == true && Base != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, Base.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (Base.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        if (miningTimer.Finished)
        {
            isIdle = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Base && isFull == true)
        {
            goldDropoff.AddData(GameplayEventData.Gold, goldStorage);
            goldDropoff.Invoke(goldDropoff.Data);


            isFull = false;
        }
        if (collision.gameObject == Mine && isFull == false)
        {
            miningTimer.Run();
            isFull = true;
            isIdle = true;
        }
    }
}
