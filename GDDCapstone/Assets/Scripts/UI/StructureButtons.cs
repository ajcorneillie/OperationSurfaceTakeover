using UnityEditor;
using UnityEngine;

public class StructureButtons : MonoBehaviour
{

    StructureButton purchaseButton;
    WallButton wallButton;

    /// <summary>
    /// Events
    /// </summary>
    GameEvent purchaseAttempt = new GameEvent();

    GameEvent purchaseAttemptWall = new GameEvent();

    [SerializeField] public PurchaseButtonEnum purchaseButtonEnum;
    [SerializeField] public WallButtonEnum wallButtonEnum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptToPlayer, purchaseAttempt);
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptToPlayerWall, purchaseAttemptWall);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseAttepmt()
    {
        purchaseAttempt.AddData(UIEventData.StructureScriptable, purchaseButton);
        purchaseAttempt.Invoke(purchaseAttempt.Data);

        purchaseAttemptWall.AddData(UIEventData.WallScriptable, wallButton);
        purchaseAttemptWall.Invoke(purchaseAttemptWall.Data);
    }

    public void Initialize(StructureButton structureButton)
    {
        purchaseButton = structureButton;
    }

    public void Initialize2(WallButton structureButton)
    {
        wallButton = structureButton;
    }
}
