using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AreYouSure : MonoBehaviour
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

    [SerializeField]
    GameObject pauseMenu; // reference to the pasue menu canvas
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference of the audio source on this script
    }

    // Update is called once per frame
    void Update()
    {
        //checks if there is input with the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NoClicked(); //runs the no clicked method
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// starts the no clicked coroutine
    /// </summary>
    public void NoClicked()
    {
        StartCoroutine(PlayAudioNoClicked());
    }

    /// <summary>
    /// starts the yes main clicked coroutine
    /// </summary>
    public void YesMainClicked()
    {
        StartCoroutine(PlayAudioYesMainClicked());
    }

    /// <summary>
    /// starts the yes level clicked coroutine
    /// </summary>
    public void YesLevelClicked()
    {
        StartCoroutine(PlayAudioYesLevelClicked());
    }

    /// <summary>
    /// runs when the script is first started
    /// </summary>
    /// <param name="PauseMenu"></param>
    public void StartMe(GameObject PauseMenu)
    {
        pauseMenu = PauseMenu; //sets the reference to the pause canvas

        gameObject.SetActive(false); //deactivates self
    }

    /// <summary>
    /// plays the audio for yes level clicked
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioYesLevelClicked()
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

        SceneManager.LoadScene("LevelSelect"); //after the audio clip is done goes to the scene level select
    }

    /// <summary>
    /// plays the audio for yes main clicked
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioYesMainClicked()
    {
        audioMenuNum = Random.Range(1, 4);//selects a random number between 3 and 1

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

        SceneManager.LoadScene("Menu"); //after the audio clip is done goes to the scene menu
    }

    /// <summary>
    /// plays the audio of the no clicked
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioNoClicked()
    {
        audioMenuNum = Random.Range(1, 4);//selects a random number between 3 and 1

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

        //after the audio clip is done sets the pause menu to active and deactivates self
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion
}
