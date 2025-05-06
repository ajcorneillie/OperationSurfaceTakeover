using System.Collections.Generic;
using UnityEngine;

public class EnemyRally : MonoBehaviour
{
    #region Fields
    int rallyTroops = 0; //sets the defaulted number of troops at the rally point to 0

    GameEvent rallyActivate = new GameEvent(); //rally activate invoker support
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //events this script listens for
        EventManager.AddListener(GameplayEvent.WaitingRally, WaitingRally);

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.RallyActivate, rallyActivate);
    }

    // Update is called once per frame
    void Update()
    {
        //if the number of rallied troops is greater than 8
        if (rallyTroops > 8)
        {
            rallyTroops = 0; //sets the number of rallied troops to 0

            rallyActivate.Invoke(); //invokes the rally activate event
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the waiting at rally event
    /// </summary>
    /// <param name="data"></param>
    private void WaitingRally(Dictionary<System.Enum, object> data)
    {
        rallyTroops++; //increases the number of rallied troops by 1
    }
    #endregion
}
