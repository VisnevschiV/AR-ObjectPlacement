using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonText : MonoBehaviour
{
    
    public TMP_Text text;

    public Color onColor;
    public Color offColor;
    bool isOn = false;

    private void OnEnable()
    {
        isOn = false;
        text.color = offColor;
    }


    public void ToggleColor()
    {
        isOn = !isOn;
        if (isOn)
        {
            text.color = onColor;
        }
        else
        {
            text.color = offColor;
        }
    }
}
