using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryCanvas : MonoBehaviour
{
    [SerializeField]
    Image Star1;
    [SerializeField]
    Image Star2;
    [SerializeField]
    Image Star3;

    [SerializeField]
    Sprite StarFilled;
    [SerializeField]
    Sprite StarEmpty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(GameplayEvent.Win, UpdateStars);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Main()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Retry()
    {
        SceneManager.LoadScene("");
    }
    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void UpdateStars(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Stars, out object output);
        int stars = (int)output;

        if (stars == 3)
        {
            Star1.sprite = StarFilled;
            Star2.sprite = StarFilled;
            Star3.sprite = StarFilled;
        }
        if (stars == 2)
        {
            Star1.sprite = StarFilled;
            Star2.sprite = StarFilled;
            Star3.sprite = StarEmpty;
        }
        if (stars == 1)
        {
            Star1.sprite = StarFilled;
            Star2.sprite = StarEmpty;
            Star3.sprite = StarEmpty;
        }
        if (stars == 0)
        {
            Star1.sprite = StarEmpty;
            Star2.sprite = StarEmpty;
            Star3.sprite = StarEmpty;
        }
    }
}
