using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/EnemyScriptable")]
public class EnemyScriptable : ScriptableObject
{
    public int Health;
    public ArmorTypeEnum ArmorTypeEnum;
    public int MoveSpeed;
    public int Damage;
    public bool TargetStructures;
    public bool isFlying;
    public Sprite mySprite;
    public int difficulty;
}
