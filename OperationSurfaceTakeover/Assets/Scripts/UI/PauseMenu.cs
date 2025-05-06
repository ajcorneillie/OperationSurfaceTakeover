using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
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

    //references to the different menus navagatable from the pause menu
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    GameObject levelCheck;
    [SerializeField]
    GameObject menuCheck;
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to the audio source of this script

        //spawns the settings menu and runs its start me method
        settingsMenu = Instantiate(settingsMenu);
        settingsMenu.GetComponent<Settings>().StartMe(gameObject);
        settingsMenu.GetComponent<VolumeSettings>().StartMe();

        //spawns the level check menu and runs its start me method
        levelCheck = Instantiate(levelCheck);
        levelCheck.GetComponent<AreYouSure>().StartMe(gameObject);

        //spawns the menu check menu and runs its start me method
        menuCheck = Instantiate(menuCheck);
        menuCheck.GetComponent<AreYouSure>().StartMe(gameObject);

        gameObject.SetActive(false); //deactivates self
    }

    // Update is called once per frame
    void Update()
    {
        //checks for input with the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeClicked(); //runs the resume button click method
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// runs the resume clicked coroutine
    /// </summary>
    public void ResumeClicked()
    {
        StartCoroutine(PlayAudioResumeClicked());
    }

    /// <summary>
    /// runs the settings clicked coroutine
    /// </summary>
    public void SettingsClicked()
    {
        StartCoroutine(PlayAudioSettingsClicked());
    }

    /// <summary>
    /// runs the level select clicked coroutine
    /// </summary>
    public void LevelSelectClicked()
    {
        StartCoroutine(PlayAudioLevelSelectedClicked());
    }
    
    /// <summary>
    /// runs the main menu clicked coroutine
    /// </summary>
    public void MainMenuClicked()
    {
        StartCoroutine(PlayAudioMainMenuClicked());
    }

    /// <summary>
    /// plays the main menu selected audio
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioMainMenuClicked()
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

        //activates the menu check canvas and deactivates self
        menuCheck.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// plays the level selected clicked audio
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioLevelSelectedClicked()
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

        //sets the level check canvas to active and deactivates self
        levelCheck.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// plays the settings clicked audio
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioSettingsClicked()
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

        //activates the settings menu and deactivates self
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// plays the resume clicked audio
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioResumeClicked()
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

        //sets time scale to 1 and deactivates self
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    #endregion
}
