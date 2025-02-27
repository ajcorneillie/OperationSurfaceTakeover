using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int health = 10;

    GameEvent updateLives = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);
        EventManager.AddInvoker(UIEvent.LivesUpdate, updateLives);

        updateLives.AddData(UIEventData.Lives, health);
        updateLives.Invoke(updateLives.Data);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemyAttack(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        data.TryGetValue(GameplayEventData.Enemy, out  output);
        GameObject enemy = (GameObject)output;
        

        if (gameObject == structure)
        {
            health = health - 1;
            Destroy(enemy);
        }

        updateLives.AddData(UIEventData.Lives,health);
        updateLives.Invoke(updateLives.Data);

        if (health <= 0)
        {
            print("You Die");
            Destroy(gameObject);
        }
    }
}
