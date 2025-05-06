using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    #region Fields
    private float moveSpeed = Constants.BasePlayerMoveSpeed; //sets the move speed to the constant

    private bool isMoving = false; //boolean for is moving or not

    float horizontalSpeed, verticalSpeed; //reference to vertical and horizontal speed

    public Vector2 moveDirection = Vector2.zero; //reference to the current dirction of movement

    Rigidbody2D rb2D; //reference to the rigid body 2d component

    Vector2 lastDirection; // reference to the last direction of movement

    private Vector3 mousePos; //reference to the current mouse position

    Vector2 mouseAimDirection = Vector2.zero; //sets the mouse aim direction to a z value of 0

    public Vector2 aimDirection; // reference to the aim direction of the mouse

    private Vector3 movementDirection; // reference to the current move direction of the object

    public float rotationSpeed = 1f; // sets default rotation speed to 1

    public float radius = 5f; // sets default redius to 5

    private float angle = 0f; // sets default angle of rotation to 0

    //boundries for player movement from the center point
    float minX = -35;
    float minY = -35;
    float maxX = 35;
    float maxY = 35;

    public int money; //reference to the current money

    //reference to unity input actions
    private PlayerControls controls;
    private InputAction playerMove;
    private InputAction mouseAim;

    //support for events this script invokes
    GameEvent purchaseAttempt = new GameEvent();
    GameEvent purchaseAttemptWall = new GameEvent();
    GameEvent currentGold = new GameEvent();

    /// <summary>
    /// Getter and Setter for move speed
    /// </summary>
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    /// <summary>
    /// Getter and Setter for is moving or not
    /// </summary>
    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    #endregion

    #region Unity Methods
    //runs when the object is enabled
    private void OnEnable()
    {
        //enables the controls of the unity input actions
        controls.Enable();
        playerMove.Enable();
        mouseAim.Enable();
    }

    //runs when the object is disabled
    private void OnDisable()
    {
        //disables the controls of the unity input actions
        controls.Disable();
        playerMove.Disable();
        mouseAim.Disable();
    }

    // Runs before the first frame
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>(); //reference to the game objects rigid body

        //support for the player controls using unity input actions
        controls = new PlayerControls();
        playerMove = controls.Player.Movement;
        mouseAim = controls.Player.MouseAim;

        //events this script listens for
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptToPlayer, PurchaseEvent);
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptToPlayerWall, PurchaseEventWall);
        EventManager.AddListener(GameplayEvent.GoldSpent, PurchaseSuccess);
        EventManager.AddListener(GameplayEvent.GoldDropoff, GoldIncrease);

        //events this script invokes
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttempt, purchaseAttempt);
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptWall, purchaseAttemptWall);
        EventManager.AddInvoker(GameplayEvent.CurrentGold, currentGold);
    }
    
    //runs once every frame
    private void FixedUpdate()
    {
        moveDirection = PlayerMove(playerMove.ReadValue<Vector2>()); //reads for the player's move direction

        lastDirection = moveDirection; //sets the last direction to the move direction

        NewMove(moveDirection); //starts moving in the new move direction
    }

    //runs once every frame
    private void Update()
    {
        //checks for input in both the horizontal and vertical axis
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            isMoving = false; // sets is moving to false
        }

        angle += rotationSpeed * Time.deltaTime; //sets the current angle to the rotation speed times time

        //determines the components of horizontal and vertical speed
        horizontalSpeed = Mathf.Cos(angle) * radius;
        verticalSpeed = Mathf.Sin(angle) * radius;

        movementDirection = new Vector3(horizontalSpeed, 0f, verticalSpeed); //sets the new movement direction to the new speeds for vertical and horizontal

        //checks if the magnitude of the movement direction is greater than the max spedd
        if (movementDirection.magnitude > moveSpeed)
        {
            movementDirection = movementDirection.normalized; //normalizes the movement so that speed does not exceed max speed
        }

    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// Moves the player based on a Vector 2
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector2 PlayerMove(Vector2 direction)
    {
        //normalizes direction
        direction = direction.normalized;
        lastDirection = direction;

        return direction; //returns the new direction
    }
    
    /// <summary>
    /// allows the selected object to move in the direction called
    /// </summary>
    /// <param name="direction"></param>
    public void NewMove(Vector2 direction)
    {
        Vector2 newPosition; //sets the new position

        newPosition = rb2D.position + direction * moveSpeed * Time.deltaTime; //sets the new position based on all the variables required

        //clamps the movement in the set bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb2D.MovePosition(newPosition); // Updates position

        isMoving = true; //sets the boolean of is moving to true
    }

    /// <summary>
    /// listens to the purchase event of a turret
    /// </summary>
    /// <param name="data"></param>
    void PurchaseEvent(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the scriptable object of the structure passed in the event
        data.TryGetValue(UIEventData.StructureScriptable, out object output);
        StructureButton structure = (StructureButton)output;

        //checks if the structure passed is null
        if (structure!= null)
        {
            int cost = structure.Cost; //sets the cost to that of the scriptable object passed

            //invokes the purchase attempt event passing in the player's current money, the structure being purchased, and the cost of the structure
            purchaseAttempt.AddData(UIEventData.StructureScriptable, structure);
            purchaseAttempt.AddData(UIEventData.Cost, cost);
            purchaseAttempt.AddData(UIEventData.PlayerMoney, money);
            purchaseAttempt.Invoke(purchaseAttempt.Data);
        }

    }

    /// <summary>
    /// listens to the purchase event of a wall
    /// </summary>
    /// <param name="data"></param>
    void PurchaseEventWall(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the scriptable object of the structure passed in the event 
        data.TryGetValue(UIEventData.WallScriptable, out object output);
        WallButton wall = (WallButton)output;

        //checks if the structure passed is null
        if (wall!= null)
        {
            int cost = wall.Cost; //sets the cost to that of the scriptable object passed

            //invokes the purchase attempt event passing in the player's current money, the structure being purchased, and the cost of the structure
            purchaseAttemptWall.AddData(UIEventData.WallScriptable, wall);
            purchaseAttemptWall.AddData(UIEventData.Cost, cost);
            purchaseAttemptWall.AddData(UIEventData.PlayerMoney, money);
            purchaseAttemptWall.Invoke(purchaseAttemptWall.Data);
        }

    }

    /// <summary>
    /// listens to the purchase success event
    /// </summary>
    /// <param name="data"></param>
    void PurchaseSuccess(Dictionary<System.Enum, object> data)
    {
        //tire to get the data on the money the structure costs
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int cost = (int)output;

        money = money - cost; //subtracts the cost of the structure from the total money

        //invokes the event for the current gold and passes in the current money and the cost of the structure as data
        currentGold.AddData(GameplayEventData.Gold, money);
        currentGold.AddData(GameplayEventData.Cost, cost);
        currentGold.Invoke(currentGold.Data);
    }

    /// <summary>
    /// listens for the money increase event
    /// </summary>
    /// <param name="data"></param>
    void GoldIncrease(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for money
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int gold = (int)output;

        money = money + gold; //increases the current money by the money passed in the event
    }
    #endregion
}
