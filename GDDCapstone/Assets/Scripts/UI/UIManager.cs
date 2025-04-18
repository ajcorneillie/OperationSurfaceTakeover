using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject structure;
    Sprite icon;

    int money = 0;

    [SerializeField]
    TextMeshProUGUI moneyText;
    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI waveText;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    StructureButton turret;
    [SerializeField]
    StructureButton machineGunTurret;
    [SerializeField]
    StructureButton flameThrower;
    [SerializeField]
    WallButton wall;

    [SerializeField]
    GameObject turretObj;
    [SerializeField]
    GameObject machineGunTurretObj;
    [SerializeField]
    GameObject flameThrowerObj;
    [SerializeField]
    GameObject wallObj;

    List<GameObject> structureButtons = new List<GameObject>();
    List<StructureButton> structureButtonsScriptables = new List<StructureButton>();

    List<GameObject> wallButtons = new List<GameObject>();
    List<WallButton> wallButton = new List<WallButton>();

    GameEvent purchaseSuccess = new GameEvent();
    GameEvent purchaseFailure = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu = Instantiate(pauseMenu);

        moneyText.text = $"Uranium: {0}";
        healthText.text = $"Lives: {10}";

        EventManager.AddListener(UIEvent.StructurePurchaseAttempt, PurchaseEvent);
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptWall, PurchaseEvent);
        EventManager.AddListener(UIEvent.LivesUpdate, UpdateLives);
        EventManager.AddListener(GameplayEvent.GoldSpent, MoneySpent);
        EventManager.AddListener(GameplayEvent.GoldDropoff, UpdateMoney);
        EventManager.AddListener(GameplayEvent.Wave, WaveUpdate);
        EventManager.AddInvoker(UIEvent.StructurePurchaseSuccess, purchaseSuccess);
        EventManager.AddInvoker(UIEvent.StructurePurchaseFailure, purchaseFailure);

        structureButtonsScriptables.Add(turret);
        structureButtonsScriptables.Add(machineGunTurret);
        structureButtonsScriptables.Add(flameThrower);
        wallButton.Add(wall);

        structureButtons.Add(turretObj);
        structureButtons.Add(machineGunTurretObj);
        structureButtons.Add(flameThrowerObj);
        wallButtons.Add(wallObj);



        structureButtons[0].GetComponent<StructureButtons>().purchaseButtonEnum = TurretButtonEnum.Turret;
        structureButtons[1].GetComponent<StructureButtons>().purchaseButtonEnum = TurretButtonEnum.MachineGunTurret;
        structureButtons[2].GetComponent<StructureButtons>().purchaseButtonEnum = TurretButtonEnum.FlameThrower;


        wallButtons[0].GetComponent<StructureButtons>().wallButtonEnum = WallButtonEnum.BaseWall;


        int index = 0;
        foreach (StructureButton scriptable in structureButtonsScriptables)
        {
            
            if (scriptable.PurchaseButtonEnum == structureButtons[index].GetComponent<StructureButtons>().purchaseButtonEnum)
            {
                structureButtons[index].GetComponent<StructureButtons>().Initialize(scriptable);
                
            }
            index++;
        }

        index = 0;
        foreach (WallButton script in wallButton)
        {

            if (script.WallButtonEnum == wallButtons[index].GetComponent<StructureButtons>().wallButtonEnum)
            {
                wallButtons[index].GetComponent<StructureButtons>().Initialize2(script);

            }
            index++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            PauseClicked();
        }
    }

    void PurchaseEvent(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.StructureScriptable, out object output);
        StructureButton structure = (StructureButton)output;


        data.TryGetValue(UIEventData.WallScriptable, out output);
        WallButton wall = (WallButton)output;

        int cost = 0;
        if (structure != null)
        {
            cost = structure.Cost;
        }
        if (wall != null)
        {
            cost = wall.Cost;
        }

        data.TryGetValue(UIEventData.PlayerMoney, out output);
        int playerMoney = (int)output;


        if (cost > playerMoney)
        {
            purchaseFailure.Invoke(purchaseFailure.Data);
        }
        else if (cost <= playerMoney)
        {
            purchaseSuccess.AddData(UIEventData.Cost, cost);
            purchaseSuccess.AddData(UIEventData.StructureScriptable, structure);
            purchaseSuccess.AddData(UIEventData.WallScriptable, wall);
            purchaseSuccess.Invoke(purchaseSuccess.Data);
        }
    }

    void UpdateLives(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.Lives, out object output);
        int lives = (int)output;

        healthText.text = $"Lives: {lives}";
    }

    void UpdateMoney(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int moneyGathered = (int)output;

        money = money + moneyGathered;

        moneyText.text = $"Uranium: {money}";
    }
    void MoneySpent(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int moneySpent = (int)output;

        money = money - moneySpent;

        moneyText.text = $"Uranium: {money}";
    }

    void WaveUpdate(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.MaxWave, out object output);
        int maxwave = (int)output;
        data.TryGetValue(GameplayEventData.Wave, out output);
        int wave = (int)output;


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

    public void PauseClicked()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

}
