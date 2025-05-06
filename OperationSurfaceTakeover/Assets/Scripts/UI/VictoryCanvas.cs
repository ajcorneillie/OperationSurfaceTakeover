using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryCanvas : MonoBehaviour
{
    #region Fields
    //references to audio clips for 3 different menu sound effects and the victory and lose songs
    [SerializeField]
    AudioClip menuSfx1;
    [SerializeField]
    AudioClip menuSfx2;
    [SerializeField]
    AudioClip menuSfx3;
    [SerializeField]
    AudioClip victorySong;
    public AudioClip loseSong;

    public AudioSource myAudioSource; //reference to this script's audio source

    int audioMenuNum = 0; //the number that applies to each menu clip to play

    public bool isLoseCanvas; //determines if the canvas is a lose canvas or not

    //reference to the stars on the menu
    [SerializeField]
    Image Star1;
    [SerializeField]
    Image Star2;
    [SerializeField]
    Image Star3;
    [SerializeField]
    Sprite StarFilled;
    [SerializeField]
    Sprite StarEmpty;

    //reference to the text correlating to the stars
    [SerializeField]
    TextMeshProUGUI starText1;
    [SerializeField]
    TextMeshProUGUI starText2;
    [SerializeField]
    TextMeshProUGUI starText3;
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to the audio source of this script

        //events this script listens for
        EventManager.AddListener(GameplayEvent.Win, UpdateStars);

        gameObject.SetActive(false); //deactivates self
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// starts the main coroutine
    /// </summary>
    public void Main()
    {
        StartCoroutine(PlayAudioMain());
    }
    /// <summary>
    /// starts the retry coroutine
    /// </summary>
    public void Retry()
    {
        StartCoroutine(PlayAudioRetry());
    }
    /// <summary>
    /// starts the level select coroutine
    /// </summary>
    public void LevelSelect()
    {

        StartCoroutine(PlayAudioLevelSelect());

    }

    /// <summary>
    /// listens for the update stars event
    /// </summary>
    /// <param name="data"></param>
    public void UpdateStars(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the stars of the level completed
        data.TryGetValue(GameplayEventData.Stars, out object output);
        int stars = (int)output;

        //tries to get the data on whether or not the level is endless
        data.TryGetValue(GameplayEventData.IsEndless, out output);
        bool isEndless = (bool)output;

        //checks if the level is not endless
        if (isEndless == false)
        {
            //checks if stars is equal to 3
            if (stars == 3)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarFilled;
                Star2.sprite = StarFilled;
                Star3.sprite = StarFilled;
            }

            //checks if stars is equal to 2
            if (stars == 2)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarFilled;
                Star2.sprite = StarFilled;
                Star3.sprite = StarEmpty;
            }

            //checks if stars is equal to 1
            if (stars == 1)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarFilled;
                Star2.sprite = StarEmpty;
                Star3.sprite = StarEmpty;
            }

            //checks if stars is equal to 0
            if (stars == 0)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarEmpty;
                Star2.sprite = StarEmpty;
                Star3.sprite = StarEmpty;
            }
        }

        //checks if level is endless
        if (isEndless == true)
        {
            //changes the three star texts to display new wave value requirements
            starText1.text = "Wave 100";
            starText2.text = "Wave 150";
            starText3.text = "Wave 200";

            //checks for how many stars there are and reacts acordingly
            if (stars >= 200)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarFilled;
                Star2.sprite = StarFilled;
                Star3.sprite = StarFilled;
            }
            else if (stars >= 150)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarFilled;
                Star2.sprite = StarFilled;
                Star3.sprite = StarEmpty;
            }
            else if (stars >= 100)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarFilled;
                Star2.sprite = StarEmpty;
                Star3.sprite = StarEmpty;
            }
            else if (stars < 100)
            {
                //sets the sprites for the 3 stars
                Star1.sprite = StarEmpty;
                Star2.sprite = StarEmpty;
                Star3.sprite = StarEmpty;
            }
        }

        //checks if lose canvas is false
        if (isLoseCanvas == false)
        {
            AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None); //creates a list of all audio sources currently playing

            //goes through all audio sources in the list
            foreach (AudioSource source in allSources)
            {
                source.Stop(); //stops current audio source
            }

            myAudioSource.PlayOneShot(victorySong); //plays victory song audio clip
        }

        //checks if lose canvas is true
        if (isLoseCanvas == true)
        {
            AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None); //creates a list of all audio sources currently playing

            //goes through all audio sources in the list
            foreach (AudioSource source in allSources)
            {
                source.Stop(); //stops current audio source
            }

            myAudioSource.PlayOneShot(loseSong);//plays victory song audio clip
        }
    }

    /// <summary>
    /// plays the level select audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioLevelSelect()
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

        SceneManager.LoadScene("LevelSelect"); //goes to the level select scene
    }
    /// <summary>
    /// plays the retry audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioRetry()
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //resets the current scene
    }

    /// <summary>
    /// plays the main audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioMain()
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

        SceneManager.LoadScene("Menu"); //goes to the menu scene
    }
    #endregion
}
