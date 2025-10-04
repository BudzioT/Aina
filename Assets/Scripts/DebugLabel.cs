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

    public void Append(string msg)
    {
        if (label != null)
        {
            label.text += msg + "\n";

            // optional: truncate if too long
            if (label.text.Length > 2000)
            {
                label.text = label.text.Substring(label.text.Length - 2000);
            }
        }
    }
}