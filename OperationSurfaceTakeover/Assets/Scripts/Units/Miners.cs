using Unity.VisualScripting;
using UnityEngine;

public class Miners : MonoBehaviour
{
    #region Fields
    //sets the move speed, work speed, and capacity of the object to the constants
    private float moveSpeed = Constants.BaseMinerMoveSpeed;
    private float workSpeed = Constants.BaseMinerWorkSpeed;
    private int capacity = Constants.Capasity;

    float horizontalSpeed, verticalSpeed; //sets vertical and horizontal speed

    public Vector2 moveDirection = Vector2.zero; //sets movement to 0

    Rigidbody2D rb2D; //reference to this object's rigid body 2d

    bool full = false; //sets default value of is full to false

    [SerializeField]
    Transform Base; //reference to the base the miner will seek out

    [SerializeField]
    Transform Mine; //reference to the mine the miner will seek out
    #endregion

    #region Unity Methods
    // Update is called once per frame
    void Update()
    {
        //checks if the miner capasity is full
        if (full == true)
        {
            transform.position = Vector3.Lerp(transform.position, Base.position, moveSpeed); //moves towards the base
        }
        if (full == false)
        {
            transform.position = Vector3.Lerp(transform.position, Mine.position, moveSpeed); //moves towards the mine
        }
    }
    #endregion
}
