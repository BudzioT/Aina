using System;
using UnityEngine;

public class ClickTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Console.WriteLine("Heyoooo");
        transform.Rotate(Vector3.left, 20);
    }
}
