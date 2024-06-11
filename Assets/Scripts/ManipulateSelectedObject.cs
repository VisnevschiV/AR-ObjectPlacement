using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ManipulateSelectedObject : MonoBehaviour
{
    public PlayerManager playerManager;
    public VectorValues vectorValues;
    
    public Slider multiplierSlider;
    
    private bool isDragging = false;
    private Vector2 initialMousePos;
    private Vector3 initialObjectPosition;
    private Vector3 initialObjectRotation;
    private Vector3 initialObjectScale;
    private Vector3 previousRotation = Vector3.zero;

    public bool onX, onY, onZ;
    public bool canMove, canRotate, canScale;
    public bool _moveGlobal = false;
    private bool _cameraPerspective = false;
    private Transform _selectedObjectTransform;
    
    public void ToggleCameraPerspective()
    {
        _cameraPerspective = !_cameraPerspective;
    }
    
    public void ToggleGlobal()
    {
        _moveGlobal = !_moveGlobal;
    }

    public void ToggleXYZ()
    {
        onX = onY = onZ = !onX;
    }
    public void ToggleX()
    {
        onX = !onX;
    }
    
    public void ToggleY()
    {
        onY = !onY;
    }

    public void ToggleZ()
    {
        onZ = !onZ;
    }

    public void ActivateScale()
    {
        Reset();
        canScale = true;
      
    }

    
    public void ActivateMove()
    {
        Reset();
        canMove = true;
        
    }

    public void ActivateRotate()
    {
        Reset();
        canRotate = true;
        
    }

    
    public void Reset()
    {
        onX = onY = onZ = false;
        canMove = canRotate = canScale = false;
        _cameraPerspective = false;
        _moveGlobal = false;
    }
    public void SetNewValues(Vector3 newValues)
    {
        if (canMove)
        {
            _selectedObjectTransform.position = newValues;
        }else if (canRotate)
        { 
            _selectedObjectTransform.eulerAngles = newValues;
        }else if (canScale)
        {
            _selectedObjectTransform.localScale = newValues;
        }
    }
    public void StartDragging()
    {
        // Get the initial mouse position and object position
        initialMousePos = Mouse.current.position.ReadValue();
        _selectedObjectTransform = playerManager.selectedObject.transform;
        initialObjectPosition = _moveGlobal? _selectedObjectTransform.position: _selectedObjectTransform.localPosition;
        initialObjectRotation =  _selectedObjectTransform.eulerAngles;
        initialObjectScale =  _selectedObjectTransform.localScale;
        previousRotation = Vector3.zero;
        isDragging = true;
    }

    
    public void StopDragging()
    {
        isDragging = false;
    }
    
    public void CancelDragging()
    {
        isDragging = false;
        _selectedObjectTransform.position = initialObjectPosition;
        _selectedObjectTransform.eulerAngles = initialObjectRotation;
        _selectedObjectTransform.localScale = initialObjectScale;
    }


    private void Update()
    {
        if (isDragging)
        {
           Drag();
        }
    }

   

    private void Drag()
    {
        
        if (canMove)
        {
            Move(CalculateNewVector());
        }else if (canScale)
        {
            Scale(CalculateNewVector());
        }else if (canRotate)
        {
            Rotate(CalculateNewVector());
        }
        
    }
    
    private Vector3 CalculateNewVector()
    {
        
            float deltaX = Mouse.current.position.ReadValue().x - initialMousePos.x;
            
            float x = onX ? deltaX * multiplierSlider.value : 0;
            float y = onY ? deltaX * multiplierSlider.value : 0;
            float z = onZ ? deltaX * multiplierSlider.value : 0;
            
            return new Vector3(x, y, z);
    }
    
    private void Move( Vector3 newVector)
    {
        if (_moveGlobal)
        {
            _selectedObjectTransform.position =  newVector + initialObjectPosition;
        }else
        {
            
            Vector3 worldOffset = _selectedObjectTransform.TransformVector(newVector);
            
            Vector3 newWorldPosition = initialObjectPosition + worldOffset;
            
            _selectedObjectTransform.position = newWorldPosition;
        }
       
        vectorValues.SetValues(_selectedObjectTransform.position);
    }

    private void Scale(Vector3 newVector)
    {
        _selectedObjectTransform.localScale = newVector + initialObjectScale;
        vectorValues.SetValues(_selectedObjectTransform.localScale);
    }

    private void Rotate(Vector3 newVector)
    {
        Vector3 rotationAxisX = _cameraPerspective ? playerManager.mainCamera.transform.right : Vector3.right;
        Vector3 rotationAxisY = _cameraPerspective ? playerManager.mainCamera.transform.up : Vector3.up;
        Vector3 rotationAxisZ = _cameraPerspective ? playerManager.mainCamera.transform.forward : Vector3.forward;

        _selectedObjectTransform.RotateAround(_selectedObjectTransform.position, rotationAxisY, newVector.y - previousRotation.y);
        _selectedObjectTransform.RotateAround(_selectedObjectTransform.position, rotationAxisX, newVector.x - previousRotation.x);
        _selectedObjectTransform.RotateAround(_selectedObjectTransform.position, rotationAxisZ, newVector.z - previousRotation.z);

        previousRotation = newVector;
        vectorValues.SetValues(_selectedObjectTransform.eulerAngles);
    }

}