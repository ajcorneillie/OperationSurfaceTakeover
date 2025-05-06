using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Settings : MonoBehaviour
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
    GameObject PauseMenu; //reference to the pause menu canvas
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to the audio source on this script
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the left mouse button is down
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
    /// sets the starting data when starting
    /// </summary>
    /// <param name="pauseMenu"></param>
    public void StartMe(GameObject pauseMenu)
    {
        PauseMenu = pauseMenu; //sets the reference to the pause menu

        gameObject.SetActive(false); //deactivates self
    }

    /// <summary>
    /// plays the no clicked audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioNoClicked()
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

        //activates the pause menu and deactivate self
        PauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion
}
