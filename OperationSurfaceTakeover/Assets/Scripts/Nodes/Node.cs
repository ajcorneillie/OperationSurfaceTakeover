using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

    
public class Node : MonoBehaviour
{
    #region Fields
    //reference to the highlight object on the tile
    [SerializeField]
    GameObject highlight;

    bool isEmpty = true; //boolean for if this tile is full or not

    GameEvent placeStructure = new GameEvent(); //support for events this script invokes
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        highlight.SetActive(false); //sets the highlight object to false

        EventManager.AddInvoker(GameplayEvent.PlaceStructure, placeStructure); //events this script invokes

        EventManager.AddListener(GameplayEvent.StructureDown, StructureDown); //events this script listens to
    }

    //checks for if the left mouse button is pressed
    public void OnMouseDown()
    {
        //checks if the current time scale is 1
        if (Time.timeScale == 1)
        {
            //checks if this tile is empty and if the mouse is over this game object
            if (isEmpty == true && !EventSystem.current.IsPointerOverGameObject())
            {
                //invokes the event for a structure being placed while passing in this tile's position and a reference to this tile's game object
                placeStructure.AddData(GameplayEventData.TilePos, transform.position);
                placeStructure.AddData(GameplayEventData.Tile, gameObject);
                placeStructure.Invoke(placeStructure.Data);
            }
        }



    }

    //checks for when the mouse enters the hit box of the tile
    public void OnMouseEnter()
    {
        //checks if the time scale is 1
        if (Time.timeScale == 1)
        {
            //checks if the mouse is currently over the tile
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                highlight.SetActive(true); //sets the highlight object to true
            }
            else
            {
                highlight.SetActive(false); //sets the highlight object to false
            }
        }


    }

    //checks for when the mouse leaves the hitbox of the tile
    public void OnMouseExit()
    {
        highlight.SetActive(false); //sets the highlight object to false
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// method for setting up the default state of the floor tile
    /// </summary>
    /// <param name="isfloorTile"></param>
    public void Initialize(bool isfloorTile)
    {
        //checks if the current tile is a floor tile
        if (isfloorTile == true)
        {
            gameObject.SetActive(true); //sets gameobject to active
        }

        //checks if the current tile is not a floor tile
        if (isfloorTile == false)
        {
            gameObject.SetActive(false); //sets game object to deactive
        }
    }

    /// <summary>
    /// resets the current tile
    /// </summary>
    public void ResetMe()
    {
        isEmpty = true; //sets the boolean of is empty to false
    }

    /// <summary>
    /// event for when a structure is being placed
    /// </summary>
    /// <param name="data"></param>
    void StructureDown(Dictionary<System.Enum, object> data)
    {
        //tries to get the data of the tile passed in the event
        data.TryGetValue(GameplayEventData.Tile, out object output);
        GameObject tile = (GameObject)output;

        //checks if the tile in the event is the same as this game object
        if (tile == gameObject)
        {
            isEmpty = false; //sets the boolean of is empty to false
        }
    }
    #endregion
}
