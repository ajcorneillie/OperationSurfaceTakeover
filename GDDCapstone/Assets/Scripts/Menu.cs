using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject settingsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Save path: " + Application.persistentDataPath);
        settingsMenu.GetComponent<Settings>().StartMe(gameObject);
        settingsMenu.GetComponent<VolumeSettings>().StartMe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMethod()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void SettingsMethod()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void HelpMethod()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void QuitMethod()
    {
        Application.Quit();
    }
}
