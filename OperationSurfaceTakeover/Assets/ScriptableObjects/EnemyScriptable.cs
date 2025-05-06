using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/EnemyScriptable")]

/// <summary>
/// Fields for the enemy Scriptable Objects
/// </summary>
public class EnemyScriptable : ScriptableObject
{
    public int Health;
    public ArmorTypeEnum ArmorTypeEnum;
    public float MoveSpeed;
    public int Damage;
    public bool TargetStructures;
    public bool isFlying;
    public Sprite mySprite;
    public int difficulty;
    public float size;
    public float attackSpeed;
}
