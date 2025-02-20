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


    public Transform player;

    private void Start()
    {
        cam.orthographicSize = startZoom;
    }

    /// <summary>
    /// Runs once every frame
    /// </summary>
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographic) 
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }

}
