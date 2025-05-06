using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class Miner : MonoBehaviour
{
    #region Fields
    [SerializeField]
    AudioClip moneyCollectionAudio; //reference to the money collect audio

    AudioSource myAudioSource; //reference to this object's audio source

    bool isFull = false; //boolean for if the miner is full

    bool isIdle = false; //boolean for if the miner is currently idle

    int mineSpeed = 3; //sets the mine speed or time spent at the mine

    float moveSpeed = 5f; //sets the movement speed of the miner

    int goldStorage = 20; //sets the amount of gold stored in the miner

    [SerializeField]
    GameObject Base; //reference to the base object

    [SerializeField]
    GameObject Mine; //reference to the mine object

    Timer miningTimer; //reference to the mining time

    GameEvent goldDropoff = new GameEvent(); //support for the events this script invokes
    #endregion

    #region Unity Events
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference of this game object's audio source

        EventManager.AddInvoker(GameplayEvent.GoldDropoff, goldDropoff); //events this script invokes

        //support for the timer for the miner to mine
        miningTimer = gameObject.AddComponent<Timer>();
        miningTimer.Duration = mineSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the miner is idle
        if (isIdle == false)
        {
            //checks if is not full and the mine exsists
            if (isFull == false && Mine != null)
            {
                //moves towards the mine object and sets rotation to face it
                transform.position = Vector3.MoveTowards(transform.position, Mine.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (Mine.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            //checks if is full and the base exsists
            if (isFull == true && Base != null)
            {
                //moves towards the base object and sets rotation to face it
                transform.position = Vector3.MoveTowards(transform.position, Base.transform.position, moveSpeed * Time.deltaTime);
                Vector2 direction = (Base.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        //checks if the mining timer has finished
        if (miningTimer.Finished)
        {
            isIdle = false; //sets the idle state to false
        }
    }

    //checks for collisions with this game object's collision box
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if the collided object is the base and if is full is true
        if (collision.gameObject == Base && isFull == true)
        {
            myAudioSource.PlayOneShot(moneyCollectionAudio); //plays the money collected audio clip

            //invokes the gold drop off event while passing in the miner's capacity as data
            goldDropoff.AddData(GameplayEventData.Gold, goldStorage);
            goldDropoff.Invoke(goldDropoff.Data);

            isFull = false; //sets is full to false
        }

        //checks if the collided object is the mine and if is full is false
        if (collision.gameObject == Mine && isFull == false)
        {
            miningTimer.Run(); //starts the mining timer

            isFull = true; //sets is full to true

            isIdle = true; //sets is idle to true
        }
    }
    #endregion
}
