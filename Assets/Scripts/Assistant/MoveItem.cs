using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveItem : MonoBehaviour
{
    // Mouse pos to track... mouse
    private Vector3 mousePos;
    // Offset: characterPos (world) - mousePos (world). Basically offset to where you click character, so that it
    // doesn't teleport, but moves according to offset
    private Vector3 offset;
    
    private Camera mainCam;
    
    
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        
    }
    
    // Track offset where player clicked the character, didn't work in OnMouseDrag
    private void OnMouseDown()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;
    }

    // Move character according to offset, works fine!!!!
    private void OnMouseDrag()
    {
        // World pos is needed! Character location is saved that way, but mouse is in screen pos
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos + offset;
    }
}
