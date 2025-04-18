using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    GameObject levelCheck;
    [SerializeField]
    GameObject menuCheck;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsMenu = Instantiate(settingsMenu);
        settingsMenu.GetComponent<Settings>().StartMe(gameObject);
        settingsMenu.GetComponent<VolumeSettings>().StartMe();

        levelCheck = Instantiate(levelCheck);
        levelCheck.GetComponent<AreYouSure>().StartMe(gameObject);

        menuCheck = Instantiate(menuCheck);
        menuCheck.GetComponent<AreYouSure>().StartMe(gameObject);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeClicked();
        }
    }

    public void ResumeClicked()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void SettingsClicked()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void LevelSelectClicked()
    {
        levelCheck.SetActive(true);
        gameObject.SetActive(false);
    }

    public void MainMenuClicked()
    {
        menuCheck.SetActive(true);
        gameObject.SetActive(false);
    }
}
