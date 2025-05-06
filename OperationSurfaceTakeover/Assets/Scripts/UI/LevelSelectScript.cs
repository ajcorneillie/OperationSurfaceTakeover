using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectScript : MonoBehaviour
{
    #region Fields
    //references to complete, uncomplete, and not ready sprites
    [SerializeField]
    Sprite uncomplete;
    [SerializeField]
    Sprite complete;
    [SerializeField]
    Sprite notReady;

    //references to this object's key, the previous object's key, and this object's level number
    public int mylevelnum;
    public string levelKey;
    public string previousLevelKey;

    //references to audio clips for 3 different menu sound effects
    [SerializeField]
    AudioClip menuSfx1;
    [SerializeField]
    AudioClip menuSfx2;
    [SerializeField]
    AudioClip menuSfx3;

    AudioSource myAudioSource; //reference to this objecta's audio source

    int audioMenuNum = 0; //the number that applies to each menu clip to play

    //references to the star images and the star complete and uncomplete sprites
    [SerializeField]
    Image star1;
    [SerializeField]
    Image star2;
    [SerializeField]
    Image star3;
    [SerializeField]
    Sprite starComplete;
    [SerializeField]
    Sprite starUncomplete;

    [SerializeField]
    TextMeshProUGUI endlessText; //reference to the text for endless mode

    //strings for the key for the stars on each stage
    public string levelStar1;
    public string levelStar2;
    public string levelStar3;

    public bool isEndless; //boolean that checks if the level is endless

    //references to stars collected and levels beaten
    int stars;
    int levelbeat;

    //support for events this script invokes
    GameEvent selected = new GameEvent();
    GameEvent start = new GameEvent();
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to the audio source on this script

        //sets the default stars and levels beaten to 0
        stars = 0;
        levelbeat = 0;

        //events this script invokes
        EventManager.AddInvoker(UIEvent.LevelSelect, selected);
        EventManager.AddInvoker(UIEvent.Start, start);

        //checks if the player prefs match the level key, previous level key, or if the current level is level 1
        if (PlayerPrefs.GetInt(levelKey, 0) == 1)
        {
            gameObject.GetComponent<Image>().sprite = complete;
            levelbeat++;
        }
        else if (levelKey == previousLevelKey)
        {
            gameObject.GetComponent<Image>().sprite = uncomplete;
        }
        else if (PlayerPrefs.GetInt(previousLevelKey, 0) == 1)
        {
            gameObject.GetComponent<Image>().sprite = uncomplete;
        }
        else if (levelKey == "Level1")
        {
            gameObject.GetComponent<Image>().sprite = uncomplete;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = notReady;
        }

        //checks if the player prefs match the key for each of the stars
        if (PlayerPrefs.GetInt(levelStar1, 0) == 1)
        {
            star1.GetComponent<Image>().sprite = starComplete;
            stars++;
        }
        if (PlayerPrefs.GetInt(levelStar2, 0) == 1)
        {
            star2.GetComponent<Image>().sprite = starComplete;
            stars++;
        }
        if (PlayerPrefs.GetInt(levelStar3, 0) == 1)
        {
            star3.GetComponent<Image>().sprite = starComplete;
            stars++;
        }

        //checks if the level is endless
        if (isEndless == true)
        {
            endlessText.text =endlessText.text + PlayerPrefs.GetInt("maxWave", 0); //sets the max wave text to the player prefs max wave
        }

        //invokes the start event and passes in the number of stars complete and if the level is completed as data
        start.AddData(UIEventData.stars, stars);
        start.AddData(UIEventData.level, levelbeat);
        start.Invoke(start.Data);
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// plays the selected coroutine
    /// </summary>
    public void Selected()
    {
        StartCoroutine(PlayAudioSelected());
    }

    /// <summary>
    /// plays the menu audio clip 
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioSelected()
    {
        audioMenuNum = Random.Range(1, 4); //selects a random number between 3 and 1

        //plays the audio clip corresponding to each number
        switch (audioMenuNum)
        {
            case 1:
                myAudioSource.clip = menuSfx1;
                myAudioSource.PlayOneShot(menuSfx1);
                break;
            case 2:
                myAudioSource.clip = menuSfx2;
                myAudioSource.PlayOneShot(menuSfx2);
                break;
            case 3:
                myAudioSource.clip = menuSfx3;
                myAudioSource.PlayOneShot(menuSfx3);
                break;
        }
        yield return new WaitForSecondsRealtime(myAudioSource.clip.length);

        //checks if the time scale is 1
        if (Time.timeScale == 1)
        {
            //checks if this game object's sprite is not the not ready sprite
            if (gameObject.GetComponent<Image>().sprite != notReady)
            {
                //invokes the selected event and passes in this level number and the game object of the level as data
                selected.AddData(UIEventData.LevelNum, mylevelnum);
                selected.AddData(UIEventData.LevelSelected, gameObject);
                selected.Invoke(selected.Data);
            }
        }
    }
    #endregion
}
