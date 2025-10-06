using System;
using Live2D.Cubism.Rendering;
using UnityEngine;

public class FadeOnHover : MonoBehaviour
{
    // Should object be see-through when hovering over it?
    private bool isSeeThrough = false;
    // Should object turn invisible right now, or not
    private bool shouldTurnInvisible = false;
    // Speed of turning visible/invisible
    public float visibilitySpeed = 0.005f;

    private CubismRenderController cubismRenderCtrl;

    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        cubismRenderCtrl = GetComponent<CubismRenderController>();
    }

    void Update()
    {
        // Turn SeeThrough flag ON/OFF when Ctrl + Alt + I are held
        if (IsCtrlHeld() && IsAltHeld() && Input.GetKeyDown(KeyCode.I))
        {
            isSeeThrough = !isSeeThrough;
            
            // If invisibility is turned off, character should always be visible
            if (!isSeeThrough)
            {
                shouldTurnInvisible = false;
            }
            
            Debug.LogError($"Changed visibility to: {isSeeThrough}");
        }

        // Turn either visible or not
        ManageVisibility();
    }

    private void OnMouseEnter()
    {
        if (isSeeThrough)
        {
            shouldTurnInvisible = true;
        }
    }

    private void OnMouseExit()
    {
        shouldTurnInvisible = false;
    }

    // Turn character either slowly invisible, or visible
    void ManageVisibility()
    {
        // Turn slowly invisible
        if (shouldTurnInvisible)
        {
            cubismRenderCtrl.Opacity = Math.Clamp(cubismRenderCtrl.Opacity - visibilitySpeed, 0.0f, 1.0f);
            
        }
        // Turn slowly visible
        else
        {
            cubismRenderCtrl.Opacity = Math.Clamp(cubismRenderCtrl.Opacity + visibilitySpeed, 0.0f, 1.0f);
        }
    }

    bool IsCtrlHeld()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    bool IsAltHeld()
    {
        return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
    }
    
    // TODO:
    // - Character is bald when lowering opacity, prolly change the method of changing it, maybe play with shader
    // - Shortcut can't be clicked outside of Unity's game window
    // - Character can't be dragged onto a second display
}
