
/// <summary>
/// enum for the event references for level events
/// </summary>
public enum LevelEvent
{
    LevelInitialized,
    InitializeObjectives,
    ObjectiveComplete,
    ClearObjectives,
    LevelComplete,
    ChangeScene,
    InitializeLevel,
    GenerateLevel
}

/// <summary>
/// enum for the data for level events
/// </summary>
public enum LevelEventData
{
    OldRoom,
    NewRoom,
    ObjectiveList,
    TileSet
}