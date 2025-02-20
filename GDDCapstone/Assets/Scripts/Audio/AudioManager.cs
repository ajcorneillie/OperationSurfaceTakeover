using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(UIEvent.StructurePurchaseFailure, PurchaseFailure);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseFailure(Dictionary<System.Enum, object> data)
    {
        print("Fail");
    }

}
