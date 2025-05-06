using UnityEngine;

[CreateAssetMenu(fileName = "WallButton", menuName = "Scriptable Objects/WallButton")]
/// <summary>
/// Fields for the wall Scriptable Objects
/// </summary>
public class WallButton : ScriptableObject
{
    public WallButtonEnum WallButtonEnum;
    public int Cost;
    public Sprite Icon;
    public GameObject Structure;
    public int Health;
    public int TileSize;
    public GameObject Image;
}
