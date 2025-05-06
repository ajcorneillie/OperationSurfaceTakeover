using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Wall : MonoBehaviour
{
    #region Fields
    //references to audio clips for death and purchasing
    [SerializeField]
    AudioClip wallDieAudio;
    [SerializeField]
    AudioClip PurchaseAudio;

    AudioSource myAudioSource; //reference to this object's audio source

    //references to this object's base stats for health, cost, object, icon, and size
    public int health;
    public int cost;
    public GameObject structure;
    public Sprite icon;
    public int tileSize;

    GameObject myTile; //reference to the tile this structure will sit on

    //support for unity input actions
    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    //support for event this script invokes
    GameEvent destroyed = new GameEvent();
    GameEvent moneyUpdate = new GameEvent();
    #endregion

    #region Unity Methods
    // Runs When this object is enabled
    private void OnEnable()
    {
        //enables unity input actions
        controls.Enable();
        place.Enable();
        cancelPlace.Enable();
    }

    // Runs When this object is disabled
    private void OnDisable()
    {
        //disables unity input actions
        controls.Disable();
        place.Disable();
        cancelPlace.Disable();
    }

    //awake runs before the first frame
    private void Awake()
    {
        //support for unity input action
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference for the audio source on this object

        myAudioSource.PlayOneShot(PurchaseAudio, 1.5f); //plays the purchase audio clip at 150 percent audio

        //events this script listens for
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.StructureDestroyed, destroyed);
        EventManager.AddInvoker(GameplayEvent.GoldSpent, moneyUpdate);

        //invokes the money update event passing in the cost of this object
        moneyUpdate.AddData(GameplayEventData.Gold, cost);
        moneyUpdate.Invoke(moneyUpdate.Data);

    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens for the enemy attack event
    /// </summary>
    /// <param name="data"></param>
    private void EnemyAttack(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the structure the enemy is attacking
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        //tries to get the data for the damage the enemy is dealing to the structure
        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

        //checks if the structure passed in the event is the same as this game object
        if (structure == gameObject)
        {
            health = health - damage; //subtracts the damage passed in this event from this wall's health

            //checks if the current health is less than or equal to 0
            if (health <= 0)
            {
                myAudioSource.PlayOneShot(wallDieAudio); //plays the death audio clip

                //invokes the destroyed event and passes in this game object as data
                destroyed.AddData(GameplayEventData.Structure, gameObject);
                destroyed.Invoke(destroyed.Data);

                myTile.GetComponent<Node>().ResetMe(); //runs the reset me method on the node this object sits on

                Destroy(gameObject); //destroys self
            }
        }
    }

    /// <summary>
    /// initializes the stats of this structure
    /// </summary>
    /// <param name="wallButton"></param>
    /// <param name="tile"></param>
    public void Initialize(WallButton wallButton, GameObject tile)
    {
        //sets stats to that of the wall scriptable object
        health = wallButton.Health;
        cost = wallButton.Cost;
        structure = wallButton.Structure;
        tileSize = wallButton.TileSize;

        myTile = tile; //sets the tile this game object sits on to that passed through the method
    }
    #endregion
}
