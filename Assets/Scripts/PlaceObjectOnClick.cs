using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceObjectOnClick : MonoBehaviour
{
    public GameObject prefabToPlace;
    public PlayerManager playerManager;
    

    public void TryPlaceObject()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        Ray ray = playerManager.mainCamera.ScreenPointToRay(mousePosition);
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Enviroment")
            {
                Instantiate(prefabToPlace, hit.point, Quaternion.identity);
            }
            else if (hit.collider.gameObject.tag == "myModel")
            {
                playerManager.SelectNewObject(hit.collider.gameObject);
            }
                
        }
    }
}
