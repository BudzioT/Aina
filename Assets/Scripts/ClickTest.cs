using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerClickHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.LogError("Rotated via UI click");
        transform.Rotate(0f, 0f, 30f);
    }
    
    public void Clicked()
    {
        //Debug.LogError("Rotated via UI click");
        transform.Rotate(0f, 0f, 30f);
    }

    private void OnMouseDown()
    {
        //Debug.LogError("Rotated via OnMouseDown");
        transform.Rotate(0f, 0f, 30f);
    }
}
