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
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 oldPos = transform.position;
        
        transform.position = mousePos + offset;
        Debug.LogError($"Offset: {offset}, Old pos: {oldPos}, New: {transform.position}, MousePos: {mousePos}");
    }
}
