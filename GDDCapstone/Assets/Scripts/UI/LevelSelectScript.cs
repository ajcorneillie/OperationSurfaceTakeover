using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{

    [SerializeField]
    Sprite uncomplete;
    [SerializeField]
    Sprite complete;
    [SerializeField]
    Sprite notReady;
    public int mylevelnum;
    public string levelKey;
    public string previousLevelKey;


    [SerializeField]
    Image star1;
    [SerializeField]
    Image star2;
    [SerializeField]
    Image star3;
    [SerializeField]
    Sprite starComplete;

    [SerializeField]
    Sprite starUncomplete;

    public string levelStar1;
    public string levelStar2;
    public string levelStar3;


    int stars;
    int levelbeat;


    GameEvent selected = new GameEvent();
    GameEvent start = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stars = 0;
        levelbeat = 0;
        EventManager.AddInvoker(UIEvent.LevelSelect, selected);
        if (PlayerPrefs.GetInt(levelKey, 0) == 1)
        {
            gameObject.GetComponent<Image>().sprite = complete;
            levelbeat++;
        }
        else if (levelKey == previousLevelKey)
        {
            gameObject.GetComponent<Image>().sprite = uncomplete;
        }
        else if (PlayerPrefs.GetInt(previousLevelKey, 0) == 1)
        {
            gameObject.GetComponent<Image>().sprite = uncomplete;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = notReady;
        }



        if (PlayerPrefs.GetInt(levelStar1, 0) == 1)
        {
            star1.GetComponent<Image>().sprite = starComplete;
            stars++;
        }
        if (PlayerPrefs.GetInt(levelStar2, 0) == 1)
        {
            star2.GetComponent<Image>().sprite = starComplete;
            stars++;
        }
        if (PlayerPrefs.GetInt(levelStar3, 0) == 1)
        {
            star3.GetComponent<Image>().sprite = starComplete;
            stars++;
        }
        EventManager.AddInvoker(UIEvent.Start, start);

        start.AddData(UIEventData.stars, stars);
        start.AddData(UIEventData.level, levelbeat);
        start.Invoke(start.Data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selected()
    {
        if (gameObject.GetComponent<Image>().sprite != notReady)
        {
            selected.AddData(UIEventData.LevelNum, mylevelnum);
            selected.AddData(UIEventData.LevelSelected, gameObject);
            selected.Invoke(selected.Data);
        }
    }
}
