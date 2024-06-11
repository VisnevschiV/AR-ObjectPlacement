using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxesButtonsManager : MonoBehaviour
{
   public  List<ToggleButtonText> myButtons = new List<ToggleButtonText>();
   
   private int _lastButton =-1;


   public void Reset()
   {
       _lastButton = -1;
   }

   public void EnableButton(int index)
    {
        if (_lastButton!=-1 && _lastButton!=index)
        {
            myButtons[_lastButton].GetComponent<Button>().onClick.Invoke();
        }

        if (index == _lastButton)
        {
            _lastButton = -1;
        }
        else
        {
            _lastButton = index;
        }
    }
}
