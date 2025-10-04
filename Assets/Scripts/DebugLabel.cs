using System;
using UnityEngine;
using TMPro;

public class DebugLabel: MonoBehaviour
{
    private TextMeshProUGUI label;

    public void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
        label.text = "Test Start";
    }

    public void Log(string msg)
    {
        if (label != null)
        {
            label.text = msg;
        }
    }
}