using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    private bool isDragging = false;
    private float initialMouseX;
    private Vector3 initialObjectPosition;
    private float multiplier = 0.01f;
    
    [SerializeField]
    private bool onX, onY, onZ;
    void Update()
    {
        // Check if the left mouse button is pressed
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartDragging();
        }
        // Check if the left mouse button is released
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopDragging();
        }

        // If dragging, update the object's position
        if (isDragging)
        {
            Drag();
        }
    }

    private void StartDragging()
    {
        // Get the initial mouse position and object position
        initialMouseX = Mouse.current.position.ReadValue().x;
        initialObjectPosition = transform.position;
        isDragging = true;
    }

    private void StopDragging()
    {
        isDragging = false;
    }

    private void Drag()
    {
        // Calculate the change in mouse position
        float currentMouseX = Mouse.current.position.ReadValue().x;
        float deltaX = currentMouseX - initialMouseX;

        float x = onX? initialObjectPosition.x + deltaX * multiplier  :transform.position.x;
        float y = onY? initialObjectPosition.y + deltaX * multiplier  :transform.position.y;
        float z = onZ? initialObjectPosition.z + deltaX * multiplier  :transform.position.z;
        
        Debug.Log(Mouse.current.position.ReadValue());
        // Move the object along the x-axis
        transform.position = new Vector3(x, y, z);
    }
}