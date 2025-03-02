using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    GameEvent nodeHit = new GameEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.NodeHit, nodeHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            nodeHit.AddData(GameplayEventData.Node, gameObject);
            nodeHit.AddData(GameplayEventData.Enemy, collision.gameObject);
            nodeHit.Invoke(nodeHit.Data);
        }
    }

}
