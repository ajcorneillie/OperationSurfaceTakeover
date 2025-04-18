using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// Fields for the zoom and speed for the camera
    /// </summary>
    public Camera cam;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float startZoom = 10f;

    bool disableMe = false;


    public Transform player;

    private void Start()
    {
        cam.orthographicSize = startZoom;
        EventManager.AddListener(GameplayEvent.LevelComplete, LevelComplete);
    }

    /// <summary>
    /// Runs once every frame
    /// </summary>
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographic && disableMe == false) 
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }

    void LevelComplete(Dictionary<System.Enum, object> data)
    {
        disableMe = true;
    }

}
