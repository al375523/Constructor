using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;

[CustomEditor(typeof(SimplifyMesh))]
public class EditorMeshSimplify : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SimplifyMesh simplifyChildren = (SimplifyMesh)target;

        if (GUILayout.Button("Simplify Mesh"))
        {
            simplifyChildren.Simplify();
        }

    }
}
