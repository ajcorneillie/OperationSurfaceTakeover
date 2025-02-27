using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{

    GameObject image;

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
        data.TryGetValue(UIEventData.StructureScriptable, out object output);
        StructureButton structure = (StructureButton)output;

        data.TryGetValue(UIEventData.WallScriptable, out output);
        WallButton wall = (WallButton)output;

        Destroy(image);
        

        if (structure != null)
        {
            image = structure.Image;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            image = Instantiate(image, mousePos, Quaternion.identity);
            image.GetComponent<TurretImage>().Initialize(structure);
        }
        if (wall != null)
        {
            image = wall.Image;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            image = Instantiate(image, mousePos, Quaternion.identity);
            image.GetComponent<WallImage>().Initialize(wall);
        }
            
        

    }
}
