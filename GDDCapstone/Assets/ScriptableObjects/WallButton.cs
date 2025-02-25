using UnityEngine;

[CreateAssetMenu(fileName = "WallButton", menuName = "Scriptable Objects/WallButton")]
public class WallButton : ScriptableObject
{
    /// <summary>
    /// Purchase Button Variables
    /// </summary>
    /// 
    public WallButtonEnum WallButtonEnum;
    public int Cost;
    public Sprite Icon;
    public GameObject Structure;
    public int Health;
    public int TileSize;
    public GameObject Image;
}
