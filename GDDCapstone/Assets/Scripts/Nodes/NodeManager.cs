using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    Grid grid;

    int sizeX = 50;
    int sizeY = 50;

    [SerializeField]
    Node tile;

    [SerializeField]
    Transform camera;



    bool isFloor = true;

    private void Awake()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                var myPosition = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                myPosition.name = $"Tile {x} {y}";
                myPosition.GetComponent<Node>().Initialize(isFloor);

            }
        }
        transform.position = camera.transform.position;
    }


}
