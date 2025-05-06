using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    #region Fields
    GameObject image; //reference to the game object that is attempting to purchase a structure
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.AddListener(UIEvent.StructurePurchaseSuccess, PurchaseSuccess); //events this script listens to
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// listens to the purchase success event
    /// </summary>
    /// <param name="data"></param>
    void PurchaseSuccess(Dictionary<System.Enum, object> data)
    {   
        //tries to get the data for the structure scriptable object purchased
        data.TryGetValue(UIEventData.StructureScriptable, out object output);
        StructureButton structure = (StructureButton)output;

        //tries to get the data for the wall scriptable object purchased
        data.TryGetValue(UIEventData.WallScriptable, out output);
        WallButton wall = (WallButton)output;

        Destroy(image); //destroys the image that purchased the structure
        
        //checks is the structure is not null
        if (structure != null)
        {
            image = structure.Image; //sets the image to the image of the structure scriptable object

            //sets the position of the image to that of the mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            image = Instantiate(image, mousePos, Quaternion.identity); //spawns the image at the mouse position

            image.GetComponent<TurretImage>().Initialize(structure); //runs the initialize method on the image in the turret image script
        }
         //checks if the wall is not null
        if (wall != null)
        {
            image = wall.Image; //sets the image to the image of the wall scriptable object

            //sets the position of the image to that of the mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            image = Instantiate(image, mousePos, Quaternion.identity); //spawns the image at the mouse position

            image.GetComponent<WallImage>().Initialize(wall); //runs the initialize method on the image in the wall image script
        }
        #endregion
    }
}
