using UnityEngine;
using UnityEngine.SceneManagement;

public class AreYouSure : MonoBehaviour
{

    [SerializeField]
    GameObject pauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NoClicked();
        }
    }


    public void NoClicked()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void YesMainClicked()
    {
        SceneManager.LoadScene("Main");
    }
    public void YesLevelClicked()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void StartMe(GameObject PauseMenu)
    {
        pauseMenu = PauseMenu;
        gameObject.SetActive(false);
    }

}
