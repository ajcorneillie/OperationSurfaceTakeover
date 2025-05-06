using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyList : MonoBehaviour
{
    #region Fields
    [SerializeField]
    AudioClip nextWaveAudio; //refernce to the audio clip of the next wave starting

    bool lastWave = false; //boolean for if last it is the last wave starting on false

    bool sentUpdate = false; //boolean for if an update as been sent out starting on false

    [SerializeField]
    AudioClip deathAudio; //refernce to the audio clip of an enemy dying

    AudioSource myAudioSource; //reference to the audio source on this game object

    List<GameObject> enemies = new List<GameObject>(); //list of enemie game objects

    GameEvent levelComplete = new GameEvent(); //level complete event invoker support
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //reference to this game object's audio source

        sentUpdate = false; //sets the state of having sent an update yet to false
        
        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.LevelComplete, levelComplete);

        //events this script listens for
        EventManager.AddListener(GameplayEvent.EnemySpawn, EnemySpawn);
        EventManager.AddListener(GameplayEvent.EnemyDeath, EnemyDeath);
        EventManager.AddListener(GameplayEvent.Wave, WaveUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        //checks if this is the last wave
        if (lastWave == true)
        {
            //checks if the current enemies in the list is 0 and an update has not been sent yet
            if(enemies.Count == 0 && sentUpdate == false)
            {
                sentUpdate = true; //sets the state of having sent an update to true

                int health = gameObject.GetComponent<Base>().health; //grabs a reference to the base's current remaining health

                //Invokes the level complete event with the bas's remaining health for data
                levelComplete.AddData(GameplayEventData.Health, health);
                levelComplete.Invoke(levelComplete.Data);
            }
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the enemy spawning event
    /// </summary>
    /// <param name="data"></param>
    void EnemySpawn(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the enemy object spawned
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        enemies.Add(enemy); //adds the enemy passed in the event to the list of enemies currently spawned
    }

    /// <summary>
    /// listens for the enemy death event
    /// </summary>
    /// <param name="data"></param>
    void EnemyDeath(Dictionary<System.Enum, object> data)
    {
        //tries to get the data on the enemy object that died
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        //if the current list of enemies contains the enemy passed in the event
        if (enemies.Contains(enemy))
        {
            myAudioSource.PlayOneShot(deathAudio, 1.5f); //play the enemy dying sound effect at 150 percent volume 

            enemies.Remove(enemy); //remove the enemy passed in the event from the list of enemies
        }
    }

    /// <summary>
    /// listens for the wave update event
    /// </summary>
    /// <param name="data"></param>
    void WaveUpdate(Dictionary<System.Enum, object> data)
    {
        //tries to get the data on the current wave
        data.TryGetValue(GameplayEventData.Wave, out object output);
        int wave = (int)output;

        //tries to get the data on the current max wave
        data.TryGetValue(GameplayEventData.MaxWave, out output);
        int maxWave = (int)output;

        // if the current wave is equal to the max wave
        if (wave == maxWave)
        {
            lastWave = true; //the last wave state is equal to true
        }

        myAudioSource.PlayOneShot(nextWaveAudio, 4f); //play the next wave audio at 400 percent volume
    }
    #endregion
}
