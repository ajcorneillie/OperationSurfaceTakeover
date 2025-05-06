using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Fields
    //references to the player dialogue box and victory canvas
    [SerializeField]
    GameObject victoryCanvas;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject dialougeBox;

    //references to level keys and the level number
    public int levelNum;
    public string levelKey;
    public string levelStar1;
    public string levelStar2;
    public string levelStar3;

    public int waves; //reference to current wave

    //support for events this script invokes
    GameEvent levelSelectSpeech = new GameEvent();
    GameEvent win = new GameEvent();
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1; //sets time scale to 1

        player.SetActive(true); //activates the player

        //events this script invokes
        EventManager.AddInvoker(UIEvent.LevelSelectSpeech, levelSelectSpeech);
        EventManager.AddInvoker(GameplayEvent.Win, win);

        //events this script listens for
        EventManager.AddListener(GameplayEvent.LevelComplete, LevelComplete);
        
        //invokes the level select speech while passing in the current level number as data
        levelSelectSpeech.AddData(UIEventData.LevelNum, levelNum);
        levelSelectSpeech.Invoke(levelSelectSpeech.Data);
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the level complete event
    /// </summary>
    /// <param name="data"></param>
    void LevelComplete(Dictionary<System.Enum, object> data)
    {
        int stars = 0; //sets the current stars to 0

        //tries to get the data of the health of the base
        data.TryGetValue(GameplayEventData.Health, out object output);
        int health = (int)output;

        //unlocks the keys to stars based on remaining health
        if (health >= 10)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
            PlayerPrefs.SetInt(levelStar2, 1);
            PlayerPrefs.SetInt(levelStar3, 1);
            stars = stars + 3;
        }
        else if (health >= 7)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
            PlayerPrefs.SetInt(levelStar2, 1);
            stars = stars + 2;
        }
        else if (health >= 3)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
            stars++;
        }

        //unlocks the level key and saves the player prefs
        PlayerPrefs.SetInt(levelKey, 1);
        PlayerPrefs.Save();

        Time.timeScale = 0; //sets time scale to 0

        victoryCanvas.SetActive(true); //sets the victory canvas to active

        //invokes the win event and passes in the is endless boolean and how many stars as data
        win.AddData(GameplayEventData.IsEndless, false);
        win.AddData(GameplayEventData.Stars, stars);
        win.Invoke(win.Data);
    }

    /// <summary>
    /// runs when the game is over in endless mode
    /// </summary>
    public void EndlessGameOver()
    {
        int stars = gameObject.GetComponent<EndlessManager>().Wave; // sets stars to the current wave

        //unlocks the keys for stars based on waves beaten
        if (stars >= 300)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
            PlayerPrefs.SetInt(levelStar2, 1);
            PlayerPrefs.SetInt(levelStar3, 1);
        }
        else if (stars >= 200)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
            PlayerPrefs.SetInt(levelStar2, 1);
        }
        else if (stars >= 100)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
        }

        //unlocks the key for the endless level
        PlayerPrefs.SetInt(levelKey, 1);

        //checks if the current max wave is larger than the play prefs max wave
        if (stars > PlayerPrefs.GetInt("maxWave", 0))
        {
            PlayerPrefs.SetInt("maxWave", stars); //set the new player max wave to current max wave
        }   

        PlayerPrefs.Save(); //saves player prefs

        Time.timeScale = 0; // time scale equals 0

        victoryCanvas.SetActive(true); //activates the victory canvas

        //invokes the win event while passing in the is endless boolean and the current stars
        win.AddData(GameplayEventData.IsEndless, true);
        win.AddData(GameplayEventData.Stars, stars);
        win.Invoke(win.Data);
    }
    #endregion
}
