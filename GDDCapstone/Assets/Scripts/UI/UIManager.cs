using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    GameEvent purchaseSuccess = new GameEvent();
    GameEvent purchaseFailure = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        EventManager.AddListener(UIEvent.StructurePurchaseAttempt, PurchaseEvent);
        EventManager.AddInvoker(UIEvent.StructurePurchaseSuccess, purchaseSuccess);
        EventManager.AddInvoker(UIEvent.StructurePurchaseFailure, purchaseFailure);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PurchaseEvent(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        data.TryGetValue(UIEventData.Cost, out output);
        int cost = (int)output;

        data.TryGetValue(UIEventData.PlayerMoney, out output);
        int playerMoney = (int)output;

        if (cost > playerMoney)
        {
            purchaseFailure.Invoke(purchaseFailure.Data);
        }
        else if (cost <= playerMoney)
        {
            purchaseSuccess.AddData(UIEventData.Cost, cost);
            purchaseSuccess.AddData(UIEventData.Structure, structure);
            purchaseSuccess.Invoke(purchaseSuccess.Data);
        }
    }
}
