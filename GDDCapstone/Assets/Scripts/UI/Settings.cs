using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField]
    GameObject PauseMenu;
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
        PauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void StartMe(GameObject pauseMenu)
    {
        PauseMenu = pauseMenu;
        gameObject.SetActive(false);
    }
}
