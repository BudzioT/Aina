using System;
using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{
    public Camera mainCamera;
    public Transform characterTransform;
    public float sensitivity = 6f;

    private bool noCharacter = false;
    
    private void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
        
        if (!characterTransform)
        {
            Debug.LogWarning("Character transform not found");
            noCharacter = true;
        }
    }

    public Vector3 GetPosition()
    {
        if (noCharacter)
        {
            return Vector3.zero;
        }
        
        // Screen positions
        Vector3 characterScreen = mainCamera.WorldToScreenPoint(characterTransform.position);
        Vector3 mouseScreen = Input.mousePosition;
        
        // Screen space offset to get proper local pos later on
        Vector2 screenOffset = new Vector2(
            mouseScreen.x - characterScreen.x,
            mouseScreen.y - characterScreen.y
        );
        
        // Normalize 
        screenOffset.x /= Screen.width;
        screenOffset.y /= Screen.height;
        
        // Scale that far as heck scale
        Vector3 localOffset = new Vector3(screenOffset.x, screenOffset.y, 0) * sensitivity;
        
        // Local space of character
        Vector3 targetLocal = characterTransform.TransformPoint(localOffset);
        
        // Debug.LogError($"Char World: {characterTransform.position}, Screen Offset: {screenOffset}, Local Offset: {localOffset}, Target World: {targetLocal}");
        
        return targetLocal;
    }

    public bool IsActive()
    {
        return true;
    }
}