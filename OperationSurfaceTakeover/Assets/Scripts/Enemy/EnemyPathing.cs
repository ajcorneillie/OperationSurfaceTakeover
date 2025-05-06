using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    #region Fields
    GameEvent nodeHit = new GameEvent(); //node hit invoker support
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.NodeHit, nodeHit); //events this script invokes
    }
    /// <summary>
    /// detects for collisions with this game object's collision hitbox
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the collided object has the tag of enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //invokes the node hit event while passing in this game object as the node and the object that collided with it as the enemy
            nodeHit.AddData(GameplayEventData.Node, gameObject);
            nodeHit.AddData(GameplayEventData.Enemy, collision.gameObject);
            nodeHit.Invoke(nodeHit.Data);
        }
    }
    #endregion

}
