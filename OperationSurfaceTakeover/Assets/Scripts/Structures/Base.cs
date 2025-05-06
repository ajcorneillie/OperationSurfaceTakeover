using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    #region Fields
    [SerializeField]
    GameObject LoseCanvas; //holds a referense to the lose canvas

    public int health = 10; //holds a reference to the health of the base

    public bool isEndless; //bolean to determine if the level is endless

    //support for events this script invokes
    GameEvent updateLives = new GameEvent();
    GameEvent enemyDeath = new GameEvent();
    GameEvent death = new GameEvent();
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //events this script listens for
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.EnemyDeath, enemyDeath);
        EventManager.AddInvoker(UIEvent.LivesUpdate, updateLives);
        EventManager.AddInvoker(GameplayEvent.EnemyDeath, death);

        LoseCanvas = Instantiate(LoseCanvas); //instantiates the lose canvas

        //invokes the lives update event while passing in the current health for data
        updateLives.AddData(UIEventData.Lives, health);
        updateLives.Invoke(updateLives.Data);
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the enemy attack event
    /// </summary>
    /// <param name="data"></param>
    void EnemyAttack(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the structure being attacked
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        //tries to get the data of the enemy attacking the structure
        data.TryGetValue(GameplayEventData.Enemy, out  output);
        GameObject enemy = (GameObject)output;
        
        //checks if the structure passed in the event is this game object
        if (gameObject == structure)
        {
            health = health - 1; //subtracts health by 1

            //invokes the enemy death event while passing in the enemy that is attacking this object as data
            enemyDeath.AddData(GameplayEventData.Enemy, enemy);
            enemyDeath.Invoke(enemyDeath.Data);

            Destroy(enemy); //destroys the enemy attaing this object
        }

        //invokes the update lives event passing in the current heath as data
        updateLives.AddData(UIEventData.Lives,health);
        updateLives.Invoke(updateLives.Data);

        //checks if the current health is less than or equal to 0
        if (health <= 0)
        {
            //checks if the level is endless
            if (isEndless == false)
            {
                AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None); //creates a list of all currently playing audio sources

                // goes through all audio sources in the list
                foreach (AudioSource source in allSources)
                {
                    source.Stop(); //stops the current audio source
                }

                Time.timeScale = 0; //sets the tie scale to 1

                LoseCanvas.SetActive(true); //sets the lose canvas to active

                LoseCanvas.GetComponent<VictoryCanvas>().myAudioSource.PlayOneShot(LoseCanvas.GetComponent<VictoryCanvas>().loseSong); //gets a reference to the victory canvas script on the lose canvas and plays the lose audio

                Destroy(gameObject); //destroys self
            }

            //checks if level is endless
            if (isEndless == true)
            {
                Time.timeScale = 0; //sets time scale to 0

                gameObject.GetComponent<LevelManager>().EndlessGameOver(); //acceses the level manager script on this game object and runs the endless game over method

                Destroy(gameObject); //destroys self
            }
        }
    }
    #endregion
}
