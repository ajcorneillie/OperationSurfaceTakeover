using UnityEngine;

[CreateAssetMenu(fileName = "StructureButton", menuName = "Scriptable Objects/StructureButton")]
public class StructureButton : ScriptableObject
{
    /// <summary>
    /// Purchase Button Variables
    /// </summary>
    /// 
    public TurretButtonEnum PurchaseButtonEnum;
    public int Cost;
    public Sprite Icon;
    public GameObject Structure;
    public int Health;
    public int Damage;
    public int TileSize;
    public float AtkSpeed;
    public float MaxRange;
    public float MinRange;
    public GameObject Projectile;
    public GameObject Image;
    public float ProjectileSpeed;

}
