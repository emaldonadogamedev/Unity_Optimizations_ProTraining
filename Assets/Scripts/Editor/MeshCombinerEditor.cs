using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();
      if(GUILayout.Button("Check Offset"))
      {
         ((MeshCombiner)target).CheckOffset();
      }
      if(GUILayout.Button("Combine"))
      {
         ((MeshCombiner)target).CombineMesh1();
      }
      

      // if (GUILayout.Button("Save"))
      // {
      //    
      // }
   }
}
