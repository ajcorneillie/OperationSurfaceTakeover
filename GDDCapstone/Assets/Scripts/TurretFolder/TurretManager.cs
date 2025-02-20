using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(UIEvent.StructurePurchaseSuccess, PurchaseSuccess);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PurchaseSuccess(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(UIEventData.Structure, out object output);
        GameObject structure = (GameObject)output;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        structure = Instantiate(structure, mousePos, Quaternion.identity);

    }
}
