using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider menuSlider;
    [SerializeField]
    Slider SFXSlider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetMenuVolume()
    {
        float volume = menuSlider.value;
        mixer.SetFloat("Menu", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("menuVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

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
}
