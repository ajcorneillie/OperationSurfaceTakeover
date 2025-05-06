using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    #region Fields
    //references to audio clips for 3 different menu sound effects
    [SerializeField]
    AudioClip menuSfx1;
    [SerializeField]
    AudioClip menuSfx2;
    [SerializeField]
    AudioClip menuSfx3;

    AudioSource myAudioSource; //reference to this objecta's audio source

    int audioMenuNum = 0; //the number that applies to each menu clip to play

    //reference to the setting and credits menu
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    GameObject creditsMenu;
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to this object's audio source

        Debug.Log("Save path: " + Application.persistentDataPath); //prints the path to persistant data

        //runs the start me method of the settings and volume settings menu
        settingsMenu.GetComponent<Settings>().StartMe(gameObject);
        settingsMenu.GetComponent<VolumeSettings>().StartMe();
    }

    // Update is called once per frame
    void Update()
    {
        //clears the player prefs data
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    PlayerPrefs.DeleteAll();
        //}
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// plays the start method coroutine
    /// </summary>
    public void StartMethod()
    {
        StartCoroutine(PlayAudioStart());
    }

    /// <summary>
    /// plays the settings method coroutine
    /// </summary>
    public void SettingsMethod()
    {
        StartCoroutine(PlayAudioSettings());
    }

    /// <summary>
    /// plays the help method coroutine
    /// </summary>
    public void HelpMethod()
    {
        StartCoroutine(PlayAudioHelp());
    }

    /// <summary>
    /// plays the quit method coroutine
    /// </summary>
    public void QuitMethod()
    {
        StartCoroutine(PlayAudioQuit());
    }

    /// <summary>
    /// plays the quit audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioQuit()
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

        Application.Quit(); //quits the game
    }

    /// <summary>
    /// plays the help audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioHelp()
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

        //activates the credits menu and deactivates self
        creditsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// plays the settings audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioSettings()
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
        yield return new WaitForSecondsRealtime(myAudioSource.clip.length + 0.1f);

        //activates the settings menu and deactivats self
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// plays the start audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioStart()
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

        SceneManager.LoadScene("LevelSelect"); //loads the level select scene
    }
    #endregion
}
