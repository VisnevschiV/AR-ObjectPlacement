using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public PlayerManager playerManager;
    [SerializeField]
    [Range(0.01f, 0.15f)]
    private float arrowLengthMultiplier;
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float coneHeightMultiplier;
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float coneRadiusMultiplier;
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float circleRadiusMultiplier;
    
    private int coneSegments = 20;
    public int circleSegments = 64;

    private Material material;

    private void CreateMaterial()
    {
        if (!material)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            material = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }
    }

    private void OnRenderObject()
    {
        if (playerManager.selectedObject == null)
        {
            return;
        }
        Transform selectedObjectTransform = playerManager.selectedObject.transform;
        CreateMaterial();
        material.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(Matrix4x4.identity);

        GL.Begin(GL.LINES);
        
        if (playerManager.manipulateSelectedObject.canMove)
        {
            if (playerManager.manipulateSelectedObject.onX)
            {
                if (playerManager.manipulateSelectedObject._moveGlobal)
                {
                    DrawArrow(selectedObjectTransform.position, Vector3.right, Color.red);   // X axis
                }
                else
                {
                    DrawArrow(selectedObjectTransform.position, selectedObjectTransform.right, Color.red);   // X axis
                }
                
               
            }
            if (playerManager.manipulateSelectedObject.onY)
            {
                if (playerManager.manipulateSelectedObject._moveGlobal)
                {
                    DrawArrow(selectedObjectTransform.position, Vector3.up, Color.green);   // X axis
                }
                else
                {
                    DrawArrow(selectedObjectTransform.position, selectedObjectTransform.up, Color.green);   // X axis
                }

               
            }
            if (playerManager.manipulateSelectedObject.onZ)
            {
                if (playerManager.manipulateSelectedObject._moveGlobal)
                {
                    DrawArrow(selectedObjectTransform.position, Vector3.forward, Color.blue);   // X axis
                }
                else
                {
                    DrawArrow(selectedObjectTransform.position, selectedObjectTransform.forward, Color.blue);   // X axis
                }

            }
            
        }
        else if (playerManager.manipulateSelectedObject.canRotate)
        {
            if (playerManager.manipulateSelectedObject.onX)
            {
                DrawCircle(selectedObjectTransform.position, Vector3.right, Color.red);
            }            
            if (playerManager.manipulateSelectedObject.onY)
            {
                DrawCircle(selectedObjectTransform.position, Vector3.up, Color.green);
            }
            if (playerManager.manipulateSelectedObject.onZ)
            {
                DrawCircle(selectedObjectTransform.position, Vector3.forward, Color.blue);
            }

        }
        else if (playerManager.manipulateSelectedObject.canScale)
        {
            if (playerManager.manipulateSelectedObject.onX)
            {
                DrawLine(selectedObjectTransform.position, selectedObjectTransform.right, Color.red);
            }
            if (playerManager.manipulateSelectedObject.onY)
            {
                DrawLine(selectedObjectTransform.position, selectedObjectTransform.up, Color.green );
            }
            if (playerManager.manipulateSelectedObject.onZ)
            {
                DrawLine(selectedObjectTransform.position, selectedObjectTransform.forward, Color.blue);
            }
        }

        GL.End();

        GL.PopMatrix();
    }

   
    
    private void DrawCircle(Vector3 center, Vector3 normal, Color color)
    {
        GL.Color(color);

        float angleStep = 360f / circleSegments;
        Vector3 perpendicularVector = Vector3.Cross(normal, Vector3.up).normalized * (circleRadiusMultiplier * GetDistanceToObject(playerManager.selectedObject.transform));
        if (perpendicularVector == Vector3.zero)
        {
            perpendicularVector = Vector3.Cross(normal, Vector3.right).normalized * (circleRadiusMultiplier * GetDistanceToObject(playerManager.selectedObject.transform));
        }
        
        
        for (int i = 0; i < circleSegments; i++)
        {
            float angle = i * angleStep;
            float nextAngle = (i + 1) * angleStep;

            Vector3 point1 = center + (Quaternion.AngleAxis(angle, normal) * perpendicularVector);
            Vector3 point2 = center + (Quaternion.AngleAxis(nextAngle, normal) * perpendicularVector);

            GL.Vertex(point1);
            GL.Vertex(point2);
        }
    }
    
    private void DrawLine(Vector3 start, Vector3 direction, Color color)
    {
        GL.Color(color);
        GL.Vertex(start);
        GL.Vertex(start + direction * (arrowLengthMultiplier * GetDistanceToObject(playerManager.selectedObject.transform)));
    }
    private void DrawArrow(Vector3 start, Vector3 direction, Color color)
    {
        GL.Color(color);
        
        Vector3 end = start + direction * (arrowLengthMultiplier * GetDistanceToObject(playerManager.selectedObject.transform));
        GL.Vertex(start);
        GL.Vertex(end);

        // Draw cone as arrow head
        DrawCone(end, direction, color);
    }
    
    
    private void DrawCone(Vector3 baseCenter, Vector3 direction, Color color)
    {
        GL.Color(color);
        
        Vector3 coneTip = baseCenter + direction.normalized * (coneHeightMultiplier * GetDistanceToObject(playerManager.selectedObject.transform));
        float angleStep = 360f / coneSegments;
        Vector3 right = Vector3.Cross(direction, Vector3.up).normalized * (coneRadiusMultiplier* GetDistanceToObject(playerManager.selectedObject.transform));
        if (right == Vector3.zero)
        {
            right = Vector3.Cross(direction, Vector3.right).normalized * (coneRadiusMultiplier* GetDistanceToObject(playerManager.selectedObject.transform));
        }
        for (int i = 0; i < coneSegments; i++)
        {
            float angle = i * angleStep;
            float nextAngle = (i + 1) * angleStep;

            Vector3 point1 = baseCenter + (Quaternion.AngleAxis(angle, direction) * right);
            Vector3 point2 = baseCenter + (Quaternion.AngleAxis(nextAngle, direction) * right);

            GL.Vertex(coneTip);
            GL.Vertex(point1);

            GL.Vertex(point1);
            GL.Vertex(point2);
        }
        
    }
    
    private float GetDistanceToObject(Transform obj)
    {
        return Vector3.Distance(playerManager.mainCamera.transform.position, obj.position);
    }

    
}
