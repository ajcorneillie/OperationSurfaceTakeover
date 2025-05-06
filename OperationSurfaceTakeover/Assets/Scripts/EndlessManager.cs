using System.Collections.Generic;
using UnityEngine;

public class EndlessManager : MonoBehaviour
{
    #region Fields
    //references to all the spawners in the endless level
    [SerializeField]
    GameObject Spawner1;
    [SerializeField]
    GameObject Spawner2;
    [SerializeField]
    GameObject Spawner3;
    [SerializeField]
    GameObject Spawner4;
    [SerializeField]
    GameObject Spawner5;
    [SerializeField]
    GameObject Spawner6;
    [SerializeField]
    GameObject Spawner7;
    [SerializeField]
    GameObject Spawner8;
    [SerializeField]
    GameObject Spawner9;

    public int Wave; //reference to current wave
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //deactivate all spawners except for the first 2
        Spawner3.SetActive(false);
        Spawner4.SetActive(false);
        Spawner5.SetActive(false);
        Spawner6.SetActive(false);
        Spawner7.SetActive(false);
        Spawner8.SetActive(false);
        Spawner9.SetActive(false);

        EventManager.AddListener(GameplayEvent.Wave, WaveUpdate); //events this script listens for
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the wave update event
    /// </summary>
    /// <param name="data"></param>
    void WaveUpdate(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the current wave
        data.TryGetValue(GameplayEventData.Wave, out object output);
        int wave = (int)output;

        Wave = wave; //sets current wave to the same as the one passed in the event

        //activates spawners bassed on what wave it is
        if (wave > 60)
        {
            Spawner3.SetActive(true);
        }
        if (wave > 120)
        {
            Spawner4.SetActive(true);
        }
        if (wave > 180)
        {
            Spawner5.SetActive(true);
        }
        if (wave > 240)
        {
            Spawner6.SetActive(true);
        }
        if (wave > 300)
        {
            Spawner7.SetActive(true);
        }
        if (wave > 360)
        {
            Spawner8.SetActive(true);
        }
        if (wave > 420)
        {
            Spawner9.SetActive(true);
        }
    }
    #endregion
}
