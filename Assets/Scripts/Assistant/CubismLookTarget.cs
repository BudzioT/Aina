using System;
using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{
    public Camera mainCamera;
    
    private void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
    }

    public Vector3 GetPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePos);
        Debug.LogError($"Look target worldpos: {targetPosition}");
        
        return targetPosition;
    }

    public bool IsActive()
    {
        return true;
    }
}
