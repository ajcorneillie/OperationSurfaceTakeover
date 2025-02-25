using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int health = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);
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
        if (health <= 0)
        {
            print("You Die");
            Destroy(gameObject);
        }
    }
}
