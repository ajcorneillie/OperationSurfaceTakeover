using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class Node : MonoBehaviour
{
    [SerializeField]
    GameObject highlight;

    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    bool isEmpty = true;

    GameEvent placeStructure = new GameEvent();

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
    private void Start()
    {
        EventManager.AddInvoker(GameplayEvent.PlaceStructure, placeStructure);
    }


    private void OnMouseDown()
    {
        if (place.triggered && isEmpty == true)
        {
            placeStructure.AddData(GameplayEventData.TilePos, transform.position);
            placeStructure.AddData(GameplayEventData.Tile, gameObject);
            placeStructure.Invoke(placeStructure.Data);
            isEmpty = false;
        }
    }
    public void Initialize(bool isfloorTile)
    {
        if (isfloorTile == true)
        {
            gameObject.SetActive(true);
        }
        if (isfloorTile == false)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    public void ResetMe()
    {
        isEmpty = true;
    }
}
