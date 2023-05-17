using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(JsonConstructor))]
public class JsonConstructorEditor : Editor
{
    public bool showingGo = true;
    JsonConstructor jsonConstructor;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("JSON CONSTRUCTOR");
        base.OnInspectorGUI();
        jsonConstructor = (JsonConstructor)target;

        if (GUILayout.Button("Save GOS"))
        {
            jsonConstructor.SaveAllGOs();
        }

        if (GUILayout.Button("Load GOS"))
        {
            jsonConstructor.InstantiateAllGOs();
        }

        /* if (jsonConstructor.name!="" & jsonConstructor.prefab!= null)
         {
             if (GUILayout.Button("Instantiate"))
             {
                 jsonConstructor.InstantiateGO();
             }

         }
         if (jsonConstructor.GosAlreadyInstantiete())
         {
             if (GUILayout.Button("Show GameObjects"))
             {
                 jsonConstructor.ShowGo(showingGo);
                 showingGo = !showingGo;
             }
             if (GUILayout.Button("Delete GameObjects"))
             {
                 jsonConstructor.DeleteGOS();
             }
         }*/

    }
}
