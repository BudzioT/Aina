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
        GetComponent<Transform>().SetLocalPositionAndRotation(new Vector3(10, 10, 10), Quaternion.Euler(20, 40, 50));
    }
}
