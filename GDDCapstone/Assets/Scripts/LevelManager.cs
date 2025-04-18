using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject victoryCanvas;
    [SerializeField]
    GameObject player;

    public string levelKey;
    public string levelStar1;
    public string levelStar2;
    public string levelStar3;

    public int waves;

    GameEvent win = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        player.SetActive(true);
        EventManager.AddListener(GameplayEvent.LevelComplete, LevelComplete);
        EventManager.AddInvoker(GameplayEvent.Win, win);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelComplete(Dictionary<System.Enum, object> data)
    {
        int stars = 0;
        data.TryGetValue(GameplayEventData.Health, out object output);
        int health = (int)output;

        if (health >= 10)
        {
            PlayerPrefs.SetInt(levelStar1, 1);
            PlayerPrefs.SetInt(levelStar2, 1);
            PlayerPrefs.SetInt(levelStar3, 1);
            stars = stars + 3;
        }
        else if (health >= 7)
        {
            PlayerPrefs.SetInt(levelStar2, 1);
            PlayerPrefs.SetInt(levelStar3, 1);
            stars = stars + 2;
        }
        else if (health >= 3)
        {
            PlayerPrefs.SetInt(levelStar3, 1);
            stars++;
        }

        PlayerPrefs.SetInt(levelKey, 1);
        PlayerPrefs.Save();
        Time.timeScale = 0;
        victoryCanvas.SetActive(true);

        win.AddData(GameplayEventData.Stars, stars);
        win.Invoke(win.Data);
    }
}
