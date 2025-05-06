using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class TurretImage : MonoBehaviour
{
    #region Fields
    //support for unity input actions
    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    GameObject structure; //reference to the structure this image will spawn

    StructureButton structureButtonScript; //reference to the button that will spawn this object for turrets

    GameEvent placeTower = new GameEvent(); //support for the events this script invokes

    WallButton wallButtonScript; //reference to the button that will spawn this object for walls
    #endregion

    #region Unity Methods
    // Runs When this object is enabled
    private void OnEnable()
    {
        //enables the unity input actions
        controls.Enable();
        place.Enable();
        cancelPlace.Enable();
    }

    // Runs When this object is disabled
    private void OnDisable()
    {
        //disables the unity input actions
        controls.Disable();
        place.Disable();
        cancelPlace.Disable();
    }

    // awake is called before the first frame
    private void Awake()
    {
        //support for unity input actions
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;

        //events this script listens for
        EventManager.AddListener(GameplayEvent.LevelComplete, LevelComplete);
        EventManager.AddListener(GameplayEvent.PlaceStructure, PlaceStructure);
        EventManager.AddListener(GameplayEvent.CurrentGold, CurrentGold);

        //events this script invokes
        EventManager.AddInvoker(GameplayEvent.StructureDown, placeTower);
    }

    // Update is called once per frame
    void Update()
    {
        //checks if there is input for the cancel place input action
        if (cancelPlace.triggered)
        {
            Destroy(gameObject); //destroys self
        }
        
        //sets this object's position to that of the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// method to initialize the object
    /// </summary>
    /// <param name="structureButton"></param>
    public void Initialize(StructureButton structureButton)
    {
        //sets the structure and the button applied to it from the scriptable object passed through
        structure = structureButton.Structure;
        structureButtonScript = structureButton;
    }

    /// <summary>
    /// listens to the event of a structure being placed
    /// </summary>
    /// <param name="data"></param>
    private void PlaceStructure(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the location the structure will be placed on
        data.TryGetValue(GameplayEventData.TilePos, out object output);
        Vector3 location = (Vector3)output;
        
        //tries to get the data of the tile the structure will be placed on
        data.TryGetValue(GameplayEventData.Tile, out output);
        GameObject tile = (GameObject)output;
        
        //invokes the place tower event while passing in the tile from the previous event as data
        placeTower.AddData(GameplayEventData.Tile, tile);
        placeTower.Invoke(placeTower.Data);

        var theStructure = Instantiate(structure, location, Quaternion.identity); //spawns the new structure at the location of the tile

        theStructure.GetComponent<Turret>().Initialize(structureButtonScript, tile); //runs the initialize method of the turret
    }

    /// <summary>
    /// listens to the event of the level being complete
    /// </summary>
    /// <param name="data"></param>
    private void LevelComplete(Dictionary<System.Enum, object> data)
    {
        Destroy(gameObject); //destroy self
    }

    /// <summary>
    /// listens to the current gold event
    /// </summary>
    /// <param name="data"></param>
    private void CurrentGold(Dictionary<System.Enum, object> data)
    {
        //tries to get the data for the current gold of the player
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int gold = (int)output;

        //tries to get the data for the cost of the structure being spawned
        data.TryGetValue(GameplayEventData.Cost, out output);
        int cost = (int)output;

        //checks if the cost of the structure is greater than the player's gold
        if (gold < cost)
        {
            Destroy(gameObject); //destroy self
        }

    }
    #endregion
}
