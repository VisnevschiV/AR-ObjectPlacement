using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VectorValues : MonoBehaviour
{
   public TMP_InputField xInput;
   public TMP_InputField yInput;
   public TMP_InputField zInput;
   
   public ManipulateSelectedObject manipulateSelectedObject;


   public void SetValues(Vector3 values)
   {
      xInput.text = values.x.ToString();
      yInput.text = values.y.ToString();
      zInput.text = values.z.ToString();
   }

   private void OnEnable()
   {
      xInput.onValueChanged.AddListener(delegate { ValuesChangedFromInput(); });
      yInput.onValueChanged.AddListener(delegate { ValuesChangedFromInput(); });
      zInput.onValueChanged.AddListener(delegate { ValuesChangedFromInput(); });
   }

   private void ValuesChangedFromInput()
   {
      try
      {
         Vector3 newValues = new Vector3(float.Parse(xInput.text), float.Parse(yInput.text), float.Parse(zInput.text));
         manipulateSelectedObject.SetNewValues(newValues);
      }
      catch (Exception e)
      {
         Console.WriteLine(e);
         throw;
      }
      
   }
}
