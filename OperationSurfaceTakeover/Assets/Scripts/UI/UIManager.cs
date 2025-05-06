using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Fields
    GameObject structure; //reference to the current structure

    Sprite icon; //reference to the structure's icon

    int money = 0; //the current money

    //Reference to the text objects for money health and waves
    [SerializeField]
    TextMeshProUGUI moneyText;
    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI waveText;

    [SerializeField]
    GameObject pauseMenu; //reference to the pause menu

    //references to scriptable objects for turrets and walls
    [SerializeField]
    StructureButton turret;
    [SerializeField]
    StructureButton machineGunTurret;
    [SerializeField]
    StructureButton flameThrower;
    [SerializeField]
    WallButton wall;

    //references to game objects for turrets and walls
    [SerializeField]
    GameObject turretObj;
    [SerializeField]
    GameObject machineGunTurretObj;
    [SerializeField]
    GameObject flameThrowerObj;
    [SerializeField]
    GameObject wallObj;

    //lists for turrets and turret scriptable objects
    List<GameObject> structureButtons = new List<GameObject>();
    List<StructureButton> structureButtonsScriptables = new List<StructureButton>();

    //lists for walls and wall scriptable objects
    List<GameObject> wallButtons = new List<GameObject>();
    List<WallButton> wallButton = new List<WallButton>();

    //support for events this script invokes
    GameEvent purchaseSuccess = new GameEvent();
    GameEvent purchaseFailure = new GameEvent();
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu = Instantiate(pauseMenu); //spawns the pause menu

        //sets the default format for the money and health text
        moneyText.text = $"Uranium: {0}";
        healthText.text = $"Lives: {10}";

        //events this script listens to
        EventManager.AddListener(UIEvent.StructurePurchaseAttempt, PurchaseEvent);
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptWall, PurchaseEvent);
        EventManager.AddListener(UIEvent.LivesUpdate, UpdateLives);
        EventManager.AddListener(GameplayEvent.GoldSpent, MoneySpent);
        EventManager.AddListener(GameplayEvent.GoldDropoff, UpdateMoney);
        EventManager.AddListener(GameplayEvent.Wave, WaveUpdate);

        //events this script invokes
        EventManager.AddInvoker(UIEvent.StructurePurchaseSuccess, purchaseSuccess);
        EventManager.AddInvoker(UIEvent.StructurePurchaseFailure, purchaseFailure);

        //adds the scriptable objects to their respective lists
        structureButtonsScriptables.Add(turret);
        structureButtonsScriptables.Add(machineGunTurret);
        structureButtonsScriptables.Add(flameThrower);
        wallButton.Add(wall);

        //adds the game objects to their respective lists
        structureButtons.Add(turretObj);
        structureButtons.Add(machineGunTurretObj);
        structureButtons.Add(flameThrowerObj);
        wallButtons.Add(wallObj);

        //sets the turret enum on the game objects in the list
        structureButtons[0].GetComponent<StructureButtons>().purchaseButtonEnum = TurretButtonEnum.Turret;
        structureButtons[1].GetComponent<StructureButtons>().purchaseButtonEnum = TurretButtonEnum.MachineGunTurret;
        structureButtons[2].GetComponent<StructureButtons>().purchaseButtonEnum = TurretButtonEnum.FlameThrower;

        //sets the wall enum on the game objects in the list
        wallButtons[0].GetComponent<StructureButtons>().wallButtonEnum = WallButtonEnum.BaseWall;

        int index = 0; //sets index to 0

        //goes through all scriptable objects in the list
        foreach (StructureButton scriptable in structureButtonsScriptables)
        {
            //checks if the scriptable object's purchase button enum is the same as the index of the current structure button's enum
            if (scriptable.PurchaseButtonEnum == structureButtons[index].GetComponent<StructureButtons>().purchaseButtonEnum)
            {
                structureButtons[index].GetComponent<StructureButtons>().Initialize(scriptable); //initializes the game object and passes in the data of the current scriptable object   
            }

            index++; //increases index by 1
        }

        index = 0;//sets index to 0

        //goes through all scriptable objects in the list
        foreach (WallButton script in wallButton)
        {
            //checks if the scriptable object's wall button enum is the same as the index of the current wall button's enum
            if (script.WallButtonEnum == wallButtons[index].GetComponent<StructureButtons>().wallButtonEnum)
            {
                wallButtons[index].GetComponent<StructureButtons>().Initialize2(script); //initializes the game object and passes in the data of the current scriptable object
            }

            index++; //increases index by 1
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks if there is input from the escape key and time scale is 1
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            PauseClicked(); //runs the pause clicked method
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens to the purchase event event
    /// </summary>
    /// <param name="data"></param>
    void PurchaseEvent(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the scriptable object of the structure button
        data.TryGetValue(UIEventData.StructureScriptable, out object output);
        StructureButton structure = (StructureButton)output;

        //tries to get the data for the scriptable object of the wall button
        data.TryGetValue(UIEventData.WallScriptable, out output);
        WallButton wall = (WallButton)output;

        //tries to get the data for the player's current money
        data.TryGetValue(UIEventData.PlayerMoney, out output);
        int playerMoney = (int)output;

        int cost = 0; //sets cost to 0

        //checks if the structure scriptable object is null
        if (structure != null)
        {
            cost = structure.Cost; //sets cost to this scriptable object's cost
        }

        //checks if the wall scriptable object is null
        if (wall != null)
        {
            cost = wall.Cost; //sets cost to this scriptable object's cost
        }

        //checks if the cost is greater than the player money
        if (cost > playerMoney)
        {
            purchaseFailure.Invoke(purchaseFailure.Data); //invokes the purchase failure event
        }
        else if (cost <= playerMoney)
        {
            //invokes the purchase success event while passing in the cost of the structure, scriptable object of the wall and structure as data
            purchaseSuccess.AddData(UIEventData.Cost, cost);
            purchaseSuccess.AddData(UIEventData.StructureScriptable, structure);
            purchaseSuccess.AddData(UIEventData.WallScriptable, wall);
            purchaseSuccess.Invoke(purchaseSuccess.Data);
        }
    }

    /// <summary>
    /// listens for the lives update event
    /// </summary>
    /// <param name="data"></param>
    void UpdateLives(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the lives
        data.TryGetValue(UIEventData.Lives, out object output);
        int lives = (int)output;

        healthText.text = $"Lives: {lives}"; // sets the health text to the lives passed in the event
    }

    /// <summary>
    /// listens to the update money event
    /// </summary>
    /// <param name="data"></param>
    void UpdateMoney(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of gold
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int moneyGathered = (int)output;

        money = money + moneyGathered; //increases the current money by the amount of money passed in the event

        moneyText.text = $"Uranium: {money}"; //sets the money text to the current money
    }

    /// <summary>
    /// listens to the money spent event
    /// </summary>
    /// <param name="data"></param>
    void MoneySpent(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the money
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int moneySpent = (int)output;

        money = money - moneySpent; //decreases the current money by the amount of money passed in the event

        moneyText.text = $"Uranium: {money}"; //sets the money text to the current money
    }

    /// <summary>
    /// listens to the wave update event
    /// </summary>
    /// <param name="data"></param>
    void WaveUpdate(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the max wave
        data.TryGetValue(GameplayEventData.MaxWave, out object output);
        int maxwave = (int)output;

        //tries to get the data of the current wave
        data.TryGetValue(GameplayEventData.Wave, out output);
        int wave = (int)output;

        //formats the text to show the current and max wave for more then to and less than 10
        if (wave < 10 && maxwave < 10)
        {
            waveText.text = $"Waves: 0{wave} / 0{maxwave}";
        }
        else if (wave < 10 && maxwave >= 10)
        {
            waveText.text = $"Waves: 0{wave} / {maxwave}";
        }
        else
        {
            waveText.text = $"Waves: {wave} / {maxwave}";
        }
    }

    /// <summary>
    /// runs when the pause button is clicked
    /// </summary>
    public void PauseClicked()
    {
        //checks if the time scale is 1
        if (Time.timeScale == 1)
        {
            pauseMenu.SetActive(true); //sets the pause menu to active

            Time.timeScale = 0; //sets time scale to 0
        }
    }
    #endregion
}
