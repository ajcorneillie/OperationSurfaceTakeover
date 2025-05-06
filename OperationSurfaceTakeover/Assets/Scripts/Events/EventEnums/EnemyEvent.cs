
/// <summary>
/// enum for the event references for enemy events
/// </summary>
public enum EnemyEvent
{
    PlayerCollision,
    AbilityPlayerCollision,
    OnKill,
    OnEliteKill
}

/// <summary>
/// enum for the data for enemy events
/// </summary>
public enum EnemyEventData
{
    Damage,
    Position,
    ExperienceAmount,
    Tier
}