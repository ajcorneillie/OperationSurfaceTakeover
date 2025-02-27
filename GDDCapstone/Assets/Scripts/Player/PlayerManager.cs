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
    /// <summary>
    /// Fields for movement and mouse support
    /// </summary>
    private float moveSpeed = Constants.BasePlayerMoveSpeed;
    private bool isMoving = false;
    float horizontalSpeed, verticalSpeed;
    public Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb2D;
    Vector2 lastDirection;
    private Vector3 mousePos;
    Vector2 mouseAimDirection = Vector2.zero;
    public Vector2 aimDirection;
    private Vector3 movementDirection;
    public float rotationSpeed = 1f;
    public float radius = 5f;
    private float angle = 0f;

    public int money;
    /// <summary>
    /// Unity Input Actions
    /// </summary>
    private PlayerControls controls;
    private InputAction playerMove;
    private InputAction mouseAim;

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
    /// <summary>
    /// Runs When this object is enabled
    /// </summary>
    private void OnEnable()
    {
        controls.Enable();
        playerMove.Enable();
        mouseAim.Enable();
    }
    /// <summary>
    /// Runs when this object is disabled
    /// </summary>
    private void OnDisable()
    {
        controls.Disable();
        playerMove.Disable();
        mouseAim.Disable();
    }

    /// <summary>
    /// Runs before the first frame
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        playerMove = controls.Player.Movement;
        mouseAim = controls.Player.MouseAim;
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptToPlayer, PurchaseEvent);
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptToPlayerWall, PurchaseEventWall);
        EventManager.AddListener(GameplayEvent.GoldSpent, PurchaseSuccess);
        EventManager.AddListener(GameplayEvent.GoldDropoff, GoldIncrease);
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttempt, purchaseAttempt);
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptWall, purchaseAttemptWall);
        EventManager.AddInvoker(GameplayEvent.CurrentGold, currentGold);

    }

    /// <summary>
    /// Runs once every frame
    /// </summary>
    private void FixedUpdate()
    {
            // Reads for player input through the component and sets it to a vector 2
            moveDirection = PlayerMove(playerMove.ReadValue<Vector2>());
            lastDirection = moveDirection;
            NewMove(moveDirection);
    }

    /// <summary>
    /// Runs once every frame
    /// </summary>
    private void Update()
    {
        // Sets movement back to false when input isnt being registered
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            isMoving = false;
        }

        angle += rotationSpeed * Time.deltaTime;
        horizontalSpeed = Mathf.Cos(angle) * radius;
        verticalSpeed = Mathf.Sin(angle) * radius;

        movementDirection = new Vector3(horizontalSpeed, 0f, verticalSpeed);

        if (movementDirection.magnitude > moveSpeed)
        {
            movementDirection = movementDirection.normalized;
        }
    }

    /// <summary>
    /// Moves the player based on a Vector 2
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector2 PlayerMove(Vector2 direction)
    {
        //Normalize Direction
        direction = direction.normalized;
        lastDirection = direction;
        return direction;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Methods that will allow the selected object to move in the direction called
    /// </summary>
    /// <param name="direction"></param>
    public void NewMove(Vector2 direction)
    {
        Vector2 newPosition;
        newPosition = rb2D.position + direction * moveSpeed * Time.deltaTime;

        // Update position
        rb2D.MovePosition(newPosition);

        isMoving = true;
    }

    void PurchaseEvent(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.StructureScriptable, out object output);
        StructureButton structure = (StructureButton)output;

        if (structure!= null)
        {
            int cost = structure.Cost;

            int playerMoney = money;

            purchaseAttempt.AddData(UIEventData.StructureScriptable, structure);
            purchaseAttempt.AddData(UIEventData.Cost, cost);
            purchaseAttempt.AddData(UIEventData.PlayerMoney, playerMoney);
            purchaseAttempt.Invoke(purchaseAttempt.Data);
        }

    }

    void PurchaseEventWall(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.WallScriptable, out object output);
        WallButton wall = (WallButton)output;

        if (wall!= null)
        {
            int cost = wall.Cost;

            int playerMoney = money;

            purchaseAttemptWall.AddData(UIEventData.WallScriptable, wall);
            purchaseAttemptWall.AddData(UIEventData.Cost, cost);
            purchaseAttemptWall.AddData(UIEventData.PlayerMoney, playerMoney);
            purchaseAttemptWall.Invoke(purchaseAttemptWall.Data);
        }

    }

    void PurchaseSuccess(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int cost = (int)output;

        money = money - cost;

        currentGold.AddData(GameplayEventData.Gold, money);
        currentGold.AddData(GameplayEventData.Cost, cost);
        currentGold.Invoke(currentGold.Data);
    }

    void GoldIncrease(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int gold = (int)output;

        money = money + gold;
        print(money);
    }
    #endregion
}
