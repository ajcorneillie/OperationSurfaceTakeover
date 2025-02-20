using UnityEditor;
using UnityEngine;

public class StructureButton : MonoBehaviour
{
    int cost = 100;

    [SerializeField]
    private GameObject structure;

    PurchaseStructure structureScriptable;

    /// <summary>
    /// Events
    /// </summary>
    GameEvent purchaseAttempt = new GameEvent();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptToPlayer, purchaseAttempt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseAttepmt()
    {
        purchaseAttempt.AddData(UIEventData.Structure, structure);
        purchaseAttempt.AddData(UIEventData.Cost, cost);
        purchaseAttempt.Invoke(purchaseAttempt.Data);
    }
}
