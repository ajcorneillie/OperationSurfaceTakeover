using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class VolumeSettings : MonoBehaviour
{
    #region Fields
    [SerializeField]
    AudioMixer mixer; //reference to the audio mixer
    [SerializeField]
    Slider musicSlider; //reference to the slider controlling the volume of the music
    [SerializeField]
    Slider menuSlider; //reference to the slider controlling the volume of the menu SFX
    [SerializeField]
    Slider SFXSlider; //reference to the slider controlling the volume of the game SFX
    #endregion

    #region Methods

    /// <summary>
    /// controls the volume of the music via playerprefs
    /// </summary>
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    /// <summary>
    /// controls the volume of the menu SFX via playerprefs
    /// </summary>
    public void SetMenuVolume()
    {
        float volume = menuSlider.value;
        mixer.SetFloat("Menu", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("menuVolume", volume);
    }
    /// <summary>
    /// controls the volume of the game SFX via playerprefs
    /// </summary>
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    /// <summary>
    /// Sets the slider positions to the correct position based of off player prefs on run
    /// </summary>
    public void StartMe()
    {
        if (PlayerPrefs.HasKey("menuVolume"))
        {
            menuSlider.value = PlayerPrefs.GetFloat("menuVolume");
            
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            
        }
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            
        }
        SetMenuVolume();
        SetMusicVolume();
        SetSFXVolume();

        gameObject.SetActive(false);
    }
    #endregion
}
