using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{
    #region Fields
    //contains references to the level text, star text, and dialogue box
    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI starText;
    [SerializeField]
    GameObject dialougeBox;

    string firstLoad = "0"; //sets the first load string to 0

    int stars; //reference to how many stars have been collected

    int level; //reference to how many levels have been completed

    int Levelnum; //reference to what level number was selected

    string currentLevel; //reference to the current level

    List<string> scenes = new List<string>(); //list of strings that are the names of scenes

    GameEvent levelSelectSpeech = new GameEvent(); //support for events this script invokes
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialougeBox.SetActive(false); //sets the dialogue box to deactive

        //sets the base stars and levels to 0
        stars = 0;
        level = 0;

        //events this script invokes
        EventManager.AddInvoker(UIEvent.LevelSelectSpeech, levelSelectSpeech);

        //events this script listens to
        EventManager.AddListener(UIEvent.LevelSelect,LevelStart);
        EventManager.AddListener(UIEvent.Start, Startup);

        //adding all the scene names into a list
        scenes.Add("Level1");
        scenes.Add("Level2");
        scenes.Add("Level3");
        scenes.Add("Level4");
        scenes.Add("Level5");
        scenes.Add("Level6");
        scenes.Add("Level7");
        scenes.Add("Level8");
        scenes.Add("Level9");
        scenes.Add("Level10");
        scenes.Add("LevelEndless");
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens to the level start event
    /// </summary>
    /// <param name="data"></param>
    public void LevelStart(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the level number 
        data.TryGetValue(UIEventData.LevelNum, out object output);
        int levelNum = (int)output;

        //tries to get the data for the level object that is selected
        data.TryGetValue(UIEventData.LevelSelected, out output);
        GameObject levelSelected = (GameObject)output;

        Levelnum = levelNum - 1; //sets level number to level number minus 1

        currentLevel = scenes[Levelnum]; //current level equals the scene level num

        SceneManager.LoadScene(currentLevel); //loads the current scene
    }

    /// <summary>
    /// runs when the main menu is selected
    /// </summary>
    public void MainMenu()
    {
        //checks if the time scale is 1
        if (Time.timeScale == 1)
        {
            SceneManager.LoadScene("Menu"); //load the menu scene
        }
    }

    /// <summary>
    /// listens for the start up event
    /// </summary>
    /// <param name="data"></param>
    public void Startup(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the current stars
        data.TryGetValue(UIEventData.stars, out object output);
        int thesestars = (int)output;

        //tries to get the data on the current levels
        data.TryGetValue(UIEventData.level, out output);
        int leveladd = (int)output;

        //sets the current stars and levels from what was passed in the event
        stars = stars + thesestars;
        level = level + leveladd;

        
        //formats the text to be displayed for both numbers less than 10 and greater than 10
        if (stars < 10)
        {
            starText.text = string.Format("0" + stars +"  /      30");
        }
        else
        {
            starText.text = string.Format("" +stars+ "  /      30");
        }
        if (level < 10) 
        {
            levelText.text = string.Format("0"+level+"  /      10");
        }
        else
        {
            levelText.text = string.Format("" + level + "  /      10");
        }

        //checks the player prefs to see if this is the first load
        if (PlayerPrefs.GetInt(firstLoad, 0) == 0)
        {
            PlayerPrefs.SetInt(firstLoad, 1); //sets the first load to false

            PlayerPrefs.Save(); //saves player prefs

            dialougeBox.SetActive(true); //sets dialogue box to active

            //invokes the level select speech passing in a level number of 0 as data
            levelSelectSpeech.AddData(UIEventData.LevelNum, 0);
            levelSelectSpeech.Invoke(levelSelectSpeech.Data);
        }

        //checks the player prefs to see if this is not the first load
        if (PlayerPrefs.GetInt(firstLoad, 0) == 1)
        {
            Time.timeScale = 1; //changes time scale to 1
        }
    }
    #endregion
}
