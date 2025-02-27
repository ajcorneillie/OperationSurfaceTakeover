using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Wall : MonoBehaviour
{
    public int health;
    public int cost;
    public GameObject structure;
    public Sprite icon;
    public int tileSize;

    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    GameEvent destroyed = new GameEvent();
    GameEvent moneyUpdate = new GameEvent();

    GameObject myTile;
    /// <summary>
    /// Runs When this object is enabled
    /// </summary>
    private void OnEnable()
    {
        controls.Enable();
        place.Enable();
        cancelPlace.Enable();
    }

    /// <summary>
    /// Runs When this object is disabled
    /// </summary>
    private void OnDisable()
    {
        controls.Disable();
        place.Disable();
        cancelPlace.Disable();
    }

    private void Awake()
    {
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EventManager.AddListener(GameplayEvent.EnemyAttack, EnemyAttack);
        EventManager.AddInvoker(GameplayEvent.StructureDestroyed, destroyed);

        EventManager.AddInvoker(GameplayEvent.GoldSpent, moneyUpdate);

        moneyUpdate.AddData(GameplayEventData.Gold, cost);
        moneyUpdate.Invoke(moneyUpdate.Data);

    }

    private void Update()
    {

    }

    private void EnemyAttack(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        data.TryGetValue(GameplayEventData.Damage, out output);
        int damage = (int)output;

        if (structure == gameObject)
        {
            health = health - damage;

            if (health <= 0)
            {
                destroyed.AddData(GameplayEventData.Structure, gameObject);
                destroyed.Invoke(destroyed.Data);
                myTile.GetComponent<Node>().ResetMe();
                Destroy(gameObject);
            }
        }


    }

    public void Initialize(WallButton wallButton, GameObject tile)
    {
        health = wallButton.Health;
        cost = wallButton.Cost;
        structure = wallButton.Structure;
        tileSize = wallButton.TileSize;

        myTile = tile;
    }

}
