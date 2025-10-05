using System;
using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{
    public Camera mainCamera;
    public Transform modelTransform;
    
    private void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
    }

    public Vector3 GetPosition()
    {
        if (!mainCamera || !modelTransform)
            return Vector3.zero;

        // Get mouse position in viewport space (-1..1 range)
        Vector3 mouseViewport = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        mouseViewport = (mouseViewport * 2f) - Vector3.one;

        // Adjust based on model position in viewport space
        Vector3 modelViewport = mainCamera.WorldToViewportPoint(modelTransform.position);
        modelViewport = (modelViewport * 2f) - Vector3.one;

        // Relative offset from model center
        Vector3 relative = mouseViewport - modelViewport;

        // Optional sensitivity tweak
        relative *= 8f;

        // Return the relative position (normalized)
        return relative;
    }

    public bool IsActive()
    {
        return true;
    }
}
