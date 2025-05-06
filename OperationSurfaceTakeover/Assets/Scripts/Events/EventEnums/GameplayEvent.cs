
/// <summary>
/// enum for the event references for enemy events
/// </summary>
public enum GameplayEvent
{
    DamageEnemy,
    BulletFired,
    BulletSpawned,
    GoldDropoff,
    BulletManage,
    SelectBullet,
    EnemyAttack,
    StructureDestroyed,
    PlaceStructure,
    GoldSpent,
    CurrentGold,
    NodeHit,
    WaitingRally,
    RallyActivate,
    LevelComplete,
    Win,
    Wave,
    StructureDown,
    EnemySpawn,
    EnemyDeath,
    Death,

}

/// <summary>
/// enum for the data for enemy events
/// </summary>
public enum GameplayEventData
{
    Enemy,
    Damage,
    Bullet,
    Gold,
    Turret,
    Structure,
    Tile,
    TilePos,
    Cost,
    Node,
    level,
    Health,
    Stars,
    Wave,
    MaxWave,
    IsEndless,
    
}