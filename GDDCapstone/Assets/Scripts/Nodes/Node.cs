using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Node : MonoBehaviour
{
    [SerializeField]
    GameObject highlight;

    StructureButtons structureButtons;

    bool isEmpty = true;

    GameEvent placeStructure = new GameEvent();
    private void Start()
    {
        highlight.SetActive(false);
        EventManager.AddInvoker(GameplayEvent.PlaceStructure, placeStructure);
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

    public void OnMouseDown()
    {
        if (isEmpty == true && !EventSystem.current.IsPointerOverGameObject())
        {
            placeStructure.AddData(GameplayEventData.TilePos, transform.position);
            placeStructure.AddData(GameplayEventData.Tile, gameObject);
            placeStructure.Invoke(placeStructure.Data);
            isEmpty = false;

        }


    }

    public void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }
        


    }
    public void OnMouseExit()
    {
        highlight.SetActive(false);

    }

    public void ResetMe()
    {
        isEmpty = true;
    }
}
