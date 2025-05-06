using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreditsCanvas : MonoBehaviour
{
    #region Fields
    [SerializeField]
    GameObject MainMenuCanvas;

    //references to audio clips for 3 different menu sound effects
    [SerializeField]
    AudioClip menuSfx1;
    [SerializeField]
    AudioClip menuSfx2;
    [SerializeField]
    AudioClip menuSfx3;

    AudioSource myAudioSource; //reference to this objecta's audio source

    int audioMenuNum = 0; //the number that applies to each menu clip to play
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference of the audio source on this script
    }
    #endregion

    #region Methods and Events

    /// <summary>
    /// starts the back button coroutine
    /// </summary>
    public void BackClicked()
    {
        StartCoroutine(PlayAudioBackClicked());
    }

    /// <summary>
    /// plays the audio for the back clicked
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioBackClicked()
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

        //sets the main menu canvas to active and deactivates self
        MainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion
}
