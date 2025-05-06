using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields
    //variables for the camera zoom amount and a reference to the camera
    public Camera cam;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float startZoom = 10f;

    bool disableMe = false; //sets the default state to not disabled

    public Transform player; //holds a reference to the player's transform
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        cam.orthographicSize = startZoom; //sets the camera start zoom amount

        EventManager.AddListener(GameplayEvent.LevelComplete, LevelComplete);//events this script listens for
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); //set the scroll to the input on the scroll wheel axis in input actions

        //checks if the camera is orthographic and not disabled
        if (cam.orthographic && disableMe == false) 
        {
            cam.orthographicSize -= scroll * zoomSpeed;//changes the zoom by the input in the scroll wheel input axis at the zoom speed

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom); //clamps the max zoom 

            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z); //sets the position of the camera to that of the player
        }
    }
    #endregion

    #region Methods and Events
    void LevelComplete(Dictionary<System.Enum, object> data)
    {
        disableMe = true; //disables the object
    }
    #endregion
}
