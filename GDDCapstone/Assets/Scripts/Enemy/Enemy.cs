using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject myself;

    int health = 20;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(GameplayEvent.DamageEnemy, Damage);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(myself);
        }
    }

    void Damage(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Enemy, out object output);
        GameObject enemy = (GameObject)output;

        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

        if (myself = enemy)
        {
            health = health - damage;
        }
    }
}
