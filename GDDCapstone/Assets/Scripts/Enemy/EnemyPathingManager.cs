using UnityEngine;

public class EnemyPathingManager : MonoBehaviour
{
    [SerializeField]
    public GameObject node1;
    [SerializeField]
    public GameObject node2;
    [SerializeField]
    public GameObject node3;
    [SerializeField]
    public GameObject node4;
    [SerializeField]
    public GameObject node5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(node1.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
