using Unity.VisualScripting;
using UnityEngine;

public class Miners : MonoBehaviour
{
    private float moveSpeed = Constants.BaseMinerMoveSpeed;
    private float workSpeed = Constants.BaseMinerWorkSpeed;
    private int capacity = Constants.Capasity;
    float horizontalSpeed, verticalSpeed;
    public Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb2D;

    bool full = false;

    [SerializeField]
    Transform Base;

    [SerializeField]
    Transform Mine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (full == true)
        {
            transform.position = Vector3.Lerp(transform.position, Base.position, moveSpeed);
        }
        if (full == false)
        {
            transform.position = Vector3.Lerp(transform.position, Mine.position, moveSpeed);
        }
    }
}
