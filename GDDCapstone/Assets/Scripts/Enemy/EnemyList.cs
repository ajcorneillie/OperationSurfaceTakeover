using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyList : MonoBehaviour
{
    bool lastWave = false;

    List<GameObject> enemies = new List<GameObject>();
    GameEvent levelComplete = new GameEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddInvoker(GameplayEvent.LevelComplete, levelComplete);
        EventManager.AddListener(GameplayEvent.EnemySpawn, EnemySpawn);
        EventManager.AddListener(GameplayEvent.EnemyDeath, EnemyDeath);
        EventManager.AddListener(GameplayEvent.Wave, WaveUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastWave == true)
        {
            if(enemies.Count == 0)
            {
                int health = gameObject.GetComponent<Base>().health;
                levelComplete.AddData(GameplayEventData.Health, health);
                levelComplete.Invoke(levelComplete.Data);
            }
        }
    }



    void EnemySpawn(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        enemies.Add(enemy);
    }

    void EnemyDeath(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    void WaveUpdate(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Wave, out object output);
        int wave = (int)output;

        data.TryGetValue(GameplayEventData.MaxWave, out output);
        int maxWave = (int)output;

        if (wave == maxWave)
        {
            lastWave = true;
        }
    }
}
