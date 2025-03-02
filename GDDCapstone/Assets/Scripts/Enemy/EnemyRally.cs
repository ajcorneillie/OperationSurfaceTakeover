using System.Collections.Generic;
using UnityEngine;

public class EnemyRally : MonoBehaviour
{
    int rallyTroops = 0;
    GameEvent rallyActivate = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(GameplayEvent.WaitingRally, WaitingRally);
        EventManager.AddInvoker(GameplayEvent.RallyActivate, rallyActivate);
    }

    // Update is called once per frame
    void Update()
    {
        if (rallyTroops > 4)
        {
            rallyTroops = 0;
            rallyActivate.Invoke();
        }
    }

    private void WaitingRally(Dictionary<System.Enum, object> data)
    {
        rallyTroops++;
    }
}
