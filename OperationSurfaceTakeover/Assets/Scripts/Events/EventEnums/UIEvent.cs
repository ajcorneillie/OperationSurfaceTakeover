
/// <summary>
/// enum for the event references for UI events
/// </summary>
public enum UIEvent
{
    StructurePurchaseAttemptToPlayer,
    StructurePurchaseAttempt,
    StructurePurchaseAttemptWall,
    StructurePurchaseSuccess,
    StructurePurchaseFailure,
    StructurePurchaseAttemptToPlayerWall,
    LivesUpdate,
    MoneyUpdate,
    LevelSelect,
    Start,
    LevelSelectSpeech,

}

/// <summary>
/// enum for the data for UI events
/// </summary>
public enum UIEventData
{
    Structure,
    Cost,
    PlayerMoney,
    StructureScriptable,
    WallScriptable,
    Lives,
    Money,
    LevelSelected,
    LevelNum,
    level,
    stars,

}
