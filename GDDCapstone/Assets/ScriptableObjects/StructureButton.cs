using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseStructure", menuName = "Scriptable Objects/PurchaseStructure")]
public class PurchaseStructure : ScriptableObject
{
    /// <summary>
    /// Purchase Button Variables
    /// </summary>
    public string Name;
    public int Cost;
    public Sprite Icon;
    public GameObject Structure;

}
