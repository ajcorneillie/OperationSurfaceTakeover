using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    Grid grid;

    int sizeX = 75;
    int sizeY = 75;

    [SerializeField]
    Node tile;

    [SerializeField]
    Transform player;



    bool isFloor = true;

    private void Awake()
    {
        for (int x = -75; x < sizeX; x++)
        {
            for (int y = -75; y < sizeY; y++)
            {
                var myPosition = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                myPosition.name = $"Tile {x} {y}";
                myPosition.GetComponent<Node>().Initialize(isFloor);

            }
        }
    }


}
