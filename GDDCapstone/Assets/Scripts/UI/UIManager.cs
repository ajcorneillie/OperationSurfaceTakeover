using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject structure;
    Sprite icon;

    [SerializeField]
    StructureButton turret;
    [SerializeField]
    WallButton wall;

    [SerializeField]
    GameObject turretObj;
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

        EventManager.AddListener(UIEvent.StructurePurchaseAttempt, PurchaseEvent);
        EventManager.AddListener(UIEvent.StructurePurchaseAttemptWall, PurchaseEvent);
        EventManager.AddInvoker(UIEvent.StructurePurchaseSuccess, purchaseSuccess);
        EventManager.AddInvoker(UIEvent.StructurePurchaseFailure, purchaseFailure);

        structureButtonsScriptables.Add(turret);
        wallButton.Add(wall);

        structureButtons.Add(turretObj);
        wallButtons.Add(wallObj);



        structureButtons[0].GetComponent<StructureButtons>().purchaseButtonEnum = PurchaseButtonEnum.Turret;


        wallButtons[0].GetComponent<StructureButtons>().wallButtonEnum = WallButtonEnum.BaseWall;


        int index = 0;
        foreach (StructureButton scriptable in structureButtonsScriptables)
        {
            
            if (scriptable.PurchaseButtonEnum == structureButtons[index].GetComponent<StructureButtons>().purchaseButtonEnum)
            {
                structureButtons[index].GetComponent<StructureButtons>().Initialize(scriptable);
                
            }
        }

        index = 0;
        foreach (WallButton script in wallButton)
        {

            if (script.WallButtonEnum == wallButtons[index].GetComponent<StructureButtons>().wallButtonEnum)
            {
                wallButtons[index].GetComponent<StructureButtons>().Initialize2(script);

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
