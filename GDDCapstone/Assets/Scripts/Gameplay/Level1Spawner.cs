using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level1Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject levelManager;
    int index = 0;

    int waveAmount;

    [SerializeField]
    GameObject BaseEnemy;
    [SerializeField]
    GameObject Enemy2;
    [SerializeField]
    GameObject Enemy3;
    [SerializeField]
    GameObject Enemy4;
    [SerializeField]
    GameObject Enemy5;


    [SerializeField]
    GameObject theBase;

    [SerializeField]
    GameObject node1;
    [SerializeField]
    GameObject node2;
    [SerializeField]
    GameObject node3;
    [SerializeField]
    GameObject node4;
    [SerializeField]
    GameObject node5;
    [SerializeField]
    GameObject rallyPoint;

    [SerializeField]
    Camera camera;

    int xoffset;

    Vector3 spawnTransform;

    [SerializeField]
    EnemyScriptable lightArmorBug;
    [SerializeField]
    EnemyScriptable heavyArmorBug;
    [SerializeField]
    EnemyScriptable flyingArmorBug;
    [SerializeField]
    EnemyScriptable flyingHeavyArmorBug;
    [SerializeField]
    EnemyScriptable invincibleArmorBug;


    [SerializeField]
    Waves wave1;
    [SerializeField]
    Waves wave2;
    [SerializeField]
    Waves wave3;
    [SerializeField]
    Waves wave4;
    [SerializeField]
    Waves wave5;
    [SerializeField]
    Waves wave6;
    [SerializeField]
    Waves wave7;
    [SerializeField]
    Waves wave8;
    [SerializeField]
    Waves wave9;
    [SerializeField]
    Waves wave10;
    [SerializeField]
    Waves wave11;
    [SerializeField]
    Waves wave12;
    [SerializeField]
    Waves wave13;
    [SerializeField]
    Waves wave14;
    [SerializeField]
    Waves wave15;
    [SerializeField]
    Waves wave16;
    [SerializeField]
    Waves wave17;
    [SerializeField]
    Waves wave18;
    [SerializeField]
    Waves wave19;
    [SerializeField]
    Waves wave20;
    [SerializeField]
    Waves wave21;
    [SerializeField]
    Waves wave22;
    [SerializeField]
    Waves wave23;
    [SerializeField]
    Waves wave24;
    [SerializeField]
    Waves wave25;
    [SerializeField]
    Waves wave26;
    [SerializeField]
    Waves wave27;
    [SerializeField]
    Waves wave28;
    [SerializeField]
    Waves wave29;
    [SerializeField]
    Waves wave30;

    List<Waves> theWave = new List<Waves>();

    Timer waveTimer;

    GameEvent levelComplete = new GameEvent();
    GameEvent wave = new GameEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int startingwave = 0;
        waveAmount = levelManager.GetComponent<LevelManager>().waves;
        waveTimer = gameObject.AddComponent<Timer>();
        waveTimer.Duration = 10f;
        waveTimer.Run();

        theWave.Add(wave1);
        theWave.Add(wave2);
        theWave.Add(wave3);
        theWave.Add(wave4);
        theWave.Add(wave5);
        theWave.Add(wave6);
        theWave.Add(wave7);
        theWave.Add(wave8);
        theWave.Add(wave9);
        theWave.Add(wave10);
        theWave.Add(wave11);
        theWave.Add(wave12);
        theWave.Add(wave13);
        theWave.Add(wave14);
        theWave.Add(wave15);
        theWave.Add(wave16);
        theWave.Add(wave17);
        theWave.Add(wave18);
        theWave.Add(wave19);
        theWave.Add(wave20);
        theWave.Add(wave21);
        theWave.Add(wave22);
        theWave.Add(wave23);
        theWave.Add(wave24);
        theWave.Add(wave25);
        theWave.Add(wave26);
        theWave.Add(wave27);
        theWave.Add(wave28);
        theWave.Add(wave29);
        theWave.Add(wave30);

        EventManager.AddInvoker(GameplayEvent.LevelComplete, levelComplete);
        EventManager.AddInvoker(GameplayEvent.Wave, wave);

        wave.AddData(GameplayEventData.Wave, startingwave);
        wave.AddData(GameplayEventData.MaxWave, waveAmount);
        wave.Invoke(wave.Data);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer.Finished)
        {
            if (index < waveAmount)
            {
                Waves currentWave = theWave[index];

                wave.AddData(GameplayEventData.Wave, index + 1);
                wave.AddData(GameplayEventData.MaxWave, waveAmount);
                wave.Invoke(wave.Data);

                int lightbugs = currentWave.Amount1;
                int heavybugs = currentWave.Amount2;
                int flyingbugs = currentWave.Amount3;
                int heavyFlyingbugs = currentWave.Amount4;
                int invinciblebugs = currentWave.Amount5;

                for (int i = 0; i < lightbugs; i++) 
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(BaseEnemy, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, lightArmorBug, camera, rallyPoint, node1, node2, node3, node4, node5);
                }
                for (int i = 0; i < heavybugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy2, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, heavyArmorBug, camera, rallyPoint, node1, node2, node3, node4, node5);
                }
                for (int i = 0; i < flyingbugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy3, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, flyingArmorBug, camera, rallyPoint, node1, node2, node3, node4, node5);
                }
                for (int i = 0; i < heavyFlyingbugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy4, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, flyingHeavyArmorBug, camera, rallyPoint, node1, node2, node3, node4, node5);
                }
                for (int i = 0; i < invinciblebugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy5, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, invincibleArmorBug, camera, rallyPoint, node1, node2, node3, node4, node5);
                }

                index++;
                waveTimer.Run();
            }
            else
            {

            }

        }
    }
}
