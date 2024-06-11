using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Camera mainCamera;

    [SerializeField]
    private InputAction clickAction;

    [HideInInspector]
    public GameObject selectedObject;

    public PlaceObjectOnClick placeObjectOnClick;
    public ManipulateSelectedObject manipulateSelectedObject;

    public GameObject canvasInitial;
    public GameObject canvasAxes;


    private void OnEnable()
    {
        clickAction.Enable();
        canvasInitial.SetActive(false);
        canvasAxes.SetActive(false);
    }


    void Update()
    {
        if (clickAction.WasPerformedThisFrame() && selectedObject == null)
        {
            placeObjectOnClick.TryPlaceObject();
        }
        else
        {
            if (clickAction.WasPerformedThisFrame())
            {
                manipulateSelectedObject.StartDragging();
            }
            else if (clickAction.WasReleasedThisFrame())
            {
                manipulateSelectedObject.StopDragging();
            }
        }
    }


    public void SelectNewObject(GameObject newObject)
    {
        selectedObject = newObject;
        canvasInitial.SetActive(true);
        canvasAxes.SetActive(false);
    }

    public void DeselectObject()
    {
        selectedObject = null;
        canvasInitial.SetActive(false);
        canvasAxes.SetActive(false);
    }

    private void OnDisable()
    {
        clickAction.Disable();
    }
}