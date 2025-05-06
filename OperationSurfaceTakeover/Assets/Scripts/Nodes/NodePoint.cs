using UnityEngine;

public class NodePoint : MonoBehaviour
{
    #region Fields
    [SerializeField]
    Collider2D areaCollider; //reference to the collider of the node enemies path find to

    Color color; //reference to the color of this object

    GameEvent nodeHit = new GameEvent(); //support for the events this script invokes
    #endregion
    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        color.a = 0f; //sets alpha of the color to 0

        gameObject.GetComponent<SpriteRenderer>().color = color; //creates a reference to this object's spriteRenderer

        EventManager.AddInvoker(GameplayEvent.NodeHit, nodeHit); //events this script invokes

    }

    // Update is called once per frame
    void Update()
    {
        //a list of all the objects inside the collider of this object
        Collider2D[] objectsInside = Physics2D.OverlapBoxAll(areaCollider.bounds.center, areaCollider.bounds.extents, 0);

        //for each object in this collider
        foreach (Collider2D obj in objectsInside)
        {
            //checks if the current object has the tag enemy
            if (obj.CompareTag("Enemy"))
            {
                //invokes the node hit event while passing in the enemy that hit the node and this game object as data
                nodeHit.AddData(GameplayEventData.Node, gameObject);
                nodeHit.AddData(GameplayEventData.Enemy, obj.gameObject);
                nodeHit.Invoke(nodeHit.Data);
            }
        }
    }
    #endregion
}
