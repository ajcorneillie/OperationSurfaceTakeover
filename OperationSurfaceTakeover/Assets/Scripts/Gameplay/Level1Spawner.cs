using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level1Spawner : MonoBehaviour
{
    #region Fields
    public bool isEndless; //boolean to determine if the current level is endless

    public bool isEndlessCounter; //boolean to determine if this spawner is going to count the waves

    float waveSeperation = 10f; //determines the time between waves

    int endlessindex = 0; //determines the index of the wave for endless mode

    int index = 0; //determines the index for all base levels

    int waveAmount; //determines what the total waves are for this level

    Vector3 spawnTransform; //the transform of where to spawn enemies

    [SerializeField]
    GameObject levelManager; //reference to the base containing the level manager

    //reference to the different enemy objects
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

    //reference to the base object
    [SerializeField]
    GameObject theBase;

    //reference to the nodes for the first path the enemies can follow
    public GameObject node1;
    public GameObject node2;
    public GameObject node3;
    public GameObject node4;
    public GameObject node5;

    //reference to the nodes for the second path the enemies can follow
    public GameObject newnode1;
    public GameObject newnode2;
    public GameObject newnode3;
    public GameObject newnode4;
    public GameObject newnode5;

    //reference to the ralley point the enamies wait at
    public GameObject rallyPoint;

    //reference to the different enemy types scriptable objects
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

    //reference to the scriptable objectas containing the enemies for each wave
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
    [SerializeField]
    Waves wave31;
    [SerializeField]
    Waves wave32;
    [SerializeField]
    Waves wave33;
    [SerializeField]
    Waves wave34;
    [SerializeField]
    Waves wave35;
    [SerializeField]
    Waves wave36;
    [SerializeField]
    Waves wave37;
    [SerializeField]
    Waves wave38;
    [SerializeField]
    Waves wave39;
    [SerializeField]
    Waves wave40;
    [SerializeField]
    Waves wave41;
    [SerializeField]
    Waves wave42;
    [SerializeField]
    Waves wave43;
    [SerializeField]
    Waves wave44;
    [SerializeField]
    Waves wave45;
    [SerializeField]
    Waves wave46;
    [SerializeField]
    Waves wave47;
    [SerializeField]
    Waves wave48;
    [SerializeField]
    Waves wave49;
    [SerializeField]
    Waves wave50;
    [SerializeField]
    Waves wave51;
    [SerializeField]
    Waves wave52;
    [SerializeField]
    Waves wave53;
    [SerializeField]
    Waves wave54;
    [SerializeField]
    Waves wave55;
    [SerializeField]
    Waves wave56;
    [SerializeField]
    Waves wave57;
    [SerializeField]
    Waves wave58;
    [SerializeField]
    Waves wave59;
    [SerializeField]
    Waves wave60;

    //list of the scriptable objects containing the enemies for the waves
    List<Waves> theWave = new List<Waves>();

    //a timer to determine time between waves
    Timer waveTimer;

    //support for events this script invokes
    GameEvent levelComplete = new GameEvent();
    GameEvent wave = new GameEvent();
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); //sets this object's rotation to 0

        int startingwave = 0; //Sets the starting wave to 0

        waveAmount = levelManager.GetComponent<LevelManager>().waves; //collects the total waves of the level from the level manager

        //timer support for the time between waves
        waveTimer = gameObject.AddComponent<Timer>();
        waveTimer.Duration = waveSeperation;
        waveTimer.Run();

        //adds the different waves to the list of waves
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
        theWave.Add(wave31);
        theWave.Add(wave32);
        theWave.Add(wave33);
        theWave.Add(wave34);
        theWave.Add(wave35);
        theWave.Add(wave36);
        theWave.Add(wave37);
        theWave.Add(wave38);
        theWave.Add(wave39);
        theWave.Add(wave40);
        theWave.Add(wave41);
        theWave.Add(wave42);
        theWave.Add(wave43);
        theWave.Add(wave44);
        theWave.Add(wave45);
        theWave.Add(wave46);
        theWave.Add(wave47);
        theWave.Add(wave48);
        theWave.Add(wave49);
        theWave.Add(wave50);
        theWave.Add(wave51);
        theWave.Add(wave52);
        theWave.Add(wave53);
        theWave.Add(wave54);
        theWave.Add(wave55);
        theWave.Add(wave56);
        theWave.Add(wave57);
        theWave.Add(wave58);
        theWave.Add(wave59);
        theWave.Add(wave60);

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.LevelComplete, levelComplete);
        EventManager.AddInvoker(GameplayEvent.Wave, wave);

        //invokes the next wave event while passing in the current wave and the max wave
        wave.AddData(GameplayEventData.Wave, startingwave);
        wave.AddData(GameplayEventData.MaxWave, waveAmount);
        wave.Invoke(wave.Data);

        //index = 19;
    }

    // Update is called once per frame
    void Update()
    {
        //if the timer between waves is finished
        if (waveTimer.Finished)
        {
            //if the index is less then the total number of waves
            if (index < waveAmount)
            {
                Waves currentWave = theWave[index]; //sets the current wave the wave of the current index

                //checks if the current level is endless
                if (isEndless == false)
                {
                    //invokes the next wave event while passing in the current wave and the max wave
                    wave.AddData(GameplayEventData.Wave, index + 1);
                    wave.AddData(GameplayEventData.MaxWave, waveAmount);
                    wave.Invoke(wave.Data);
                }

                //collects the number of each enemy type for the givin wave
                int lightbugs = currentWave.Amount1;
                int heavybugs = currentWave.Amount2;
                int flyingbugs = currentWave.Amount3;
                int heavyFlyingbugs = currentWave.Amount4;
                int invinciblebugs = currentWave.Amount5;

                //goes through all the enemies of type 1 for the wave
                for (int i = 0; i < lightbugs; i++) 
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(BaseEnemy, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, lightArmorBug, gameObject, index);
                }

                //goes through all the enemies of type 2 for the wave
                for (int i = 0; i < heavybugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy2, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, heavyArmorBug, gameObject, index);
                }

                //goes through all the enemies of type 3 for the wave
                for (int i = 0; i < flyingbugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy3, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, flyingArmorBug, gameObject, index);
                }

                //goes through all the enemies of type 4 for the wave
                for (int i = 0; i < heavyFlyingbugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy4, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, flyingHeavyArmorBug, gameObject, index);
                }

                //goes through all the enemies of type 5 for the wave
                for (int i = 0; i < invinciblebugs; i++)
                {
                    spawnTransform = transform.position;
                    GameObject Enemy = Instantiate(Enemy5, spawnTransform, Quaternion.identity);
                    Enemy.GetComponent<Enemy>().Initialize(theBase, invincibleArmorBug, gameObject, index);
                }

                index++; //increases the current index by 1

                waveTimer.Run(); //runs the timer between waves
            }

            //checks if the current wave is greater than the mas and if the current level is endless
            if (index >= waveAmount && isEndless == true)
            {
                index = 50; //sets the current wave to 50
            }

            //checks if this object is the wave counter for endless mode and if endless mode is true
            if (isEndlessCounter == true && isEndless == true)
            {
                endlessindex++; //increases the wave count of endless mode

                //invokes the next wave event passing in the current endless wave and the max wave of 10000
                wave.AddData(GameplayEventData.Wave, endlessindex);
                wave.AddData(GameplayEventData.MaxWave, 10000);
                wave.Invoke(wave.Data);
            }

        }
    }
    #endregion
}
