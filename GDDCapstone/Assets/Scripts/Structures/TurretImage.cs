using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class TurretImage : MonoBehaviour
{

    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    GameObject structure;

    StructureButton structureButtonScript;

    WallButton wallButtonScript;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;

        EventManager.AddListener(GameplayEvent.PlaceStructure, PlaceStructure);
        EventManager.AddListener(GameplayEvent.CurrentGold, CurrentGold);
    }

    // Update is called once per frame
    void Update()
    {
        if (cancelPlace.triggered)
        {
            Destroy(gameObject);
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    public void Initialize(StructureButton structureButton)
    {
        structure = structureButton.Structure;
        structureButtonScript = structureButton;
    }

    private void PlaceStructure(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.TilePos, out object output);
        Vector3 location = (Vector3)output;

        data.TryGetValue(GameplayEventData.Tile, out output);
        GameObject tile = (GameObject)output;

        var theStructure = Instantiate(structure, location, Quaternion.identity);
        theStructure.GetComponent<Turret>().Initialize(structureButtonScript, tile);
    }

    private void CurrentGold(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Gold, out object output);
        int gold = (int)output;

        data.TryGetValue(GameplayEventData.Cost, out output);
        int cost = (int)output;

        if (gold < cost)
        {
            Destroy(gameObject);
        }

    }


}
