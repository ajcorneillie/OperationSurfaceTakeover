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

public enum LevelEventData
{
    OldRoom,
    NewRoom,
    ObjectiveList,
    TileSet
}