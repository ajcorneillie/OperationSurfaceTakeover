using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI starText;


    int stars;
    int level;

    int Levelnum;
    
    string currentLevel;
    List<string> scenes = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stars = 0;
        level = 0;

        EventManager.AddListener(UIEvent.LevelSelect,LevelStart);
        scenes.Add("Level1");
        scenes.Add("Level2");
        scenes.Add("Level3");
        scenes.Add("Level4");
        scenes.Add("Level5");
        scenes.Add("Level6");
        scenes.Add("Level7");
        scenes.Add("Level8");
        scenes.Add("Level9");
        scenes.Add("Level10");
        scenes.Add("Level11");
        scenes.Add("Level12");
        scenes.Add("Level13");
        scenes.Add("Level14");
        scenes.Add("Level15");
        scenes.Add("Level16");
        scenes.Add("Level17");
        scenes.Add("Level18");
        scenes.Add("Level19");
        scenes.Add("Level20");
        scenes.Add("LevelEndless");

        EventManager.AddListener(UIEvent.Start, Startup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelStart(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.LevelNum, out object output);
        int levelNum = (int)output;
        data.TryGetValue(UIEventData.LevelSelected, out output);
        GameObject levelSelected = (GameObject)output;

        Levelnum = levelNum - 1;
        currentLevel = scenes[Levelnum];

        SceneManager.LoadScene(currentLevel);

    }

    public void Startup(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.stars, out object output);
        int thesestars = (int)output;
        data.TryGetValue(UIEventData.level, out output);
        int leveladd = (int)output;

        stars = stars + thesestars;
        level = level + leveladd;

        
        if (stars < 10)
        {
            starText.text = string.Format("0" + stars +"  /      60");
        }
        else
        {
            starText.text = string.Format("" +stars+ "  /      60");
        }
        if (level < 10) 
        {
            levelText.text = string.Format("0"+level+"  /      20");
        }
        else
        {
            levelText.text = string.Format("" + level + "  /      20");
        }
        
    }
}
