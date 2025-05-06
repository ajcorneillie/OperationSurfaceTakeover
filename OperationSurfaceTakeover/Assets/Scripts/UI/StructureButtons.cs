using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class StructureButtons : MonoBehaviour
{
    #region Fields
    //references to the structure button and wall button scriptable objects
    StructureButton purchaseButton;
    WallButton wallButton;

    //support for events invoked by this script
    GameEvent purchaseAttempt = new GameEvent();
    GameEvent purchaseAttemptWall = new GameEvent();

    //references to the enum of different scriptable objects
    [SerializeField] public TurretButtonEnum purchaseButtonEnum;
    [SerializeField] public WallButtonEnum wallButtonEnum;
    [SerializeField] public UnitButtonEnum unitButtonEnum;
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //events that this script invokes
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptToPlayer, purchaseAttempt);
        EventManager.AddInvoker(UIEvent.StructurePurchaseAttemptToPlayerWall, purchaseAttemptWall);
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// attempts to purchase a structure
    /// </summary>
    public void PurchaseAttepmt()
    {
        //checks if the time scale is 1
        if (Time.timeScale == 1)
        {
            //invokes the purchase attampt event and passes in the purchase button as data
            purchaseAttempt.AddData(UIEventData.StructureScriptable, purchaseButton);
            purchaseAttempt.Invoke(purchaseAttempt.Data);

            //invokes the purchase attampt wall event and passes in the wall button as data
            purchaseAttemptWall.AddData(UIEventData.WallScriptable, wallButton);
            purchaseAttemptWall.Invoke(purchaseAttemptWall.Data);
        } 
    }

    /// <summary>
    /// first initialization method to set the structure button
    /// </summary>
    /// <param name="structureButton"></param>
    public void Initialize(StructureButton structureButton)
    {
        purchaseButton = structureButton;
    }

    /// <summary>
    /// first initialization method to set the wall button
    /// </summary>
    /// <param name="structureButton"></param>
    public void Initialize2(WallButton structureButton)
    {
        wallButton = structureButton;
    }
    #endregion
}
