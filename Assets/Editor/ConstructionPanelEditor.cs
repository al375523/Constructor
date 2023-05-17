using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConstructionPanel))]
public class ConstructionPanelEditor : Editor
{
    private ConstructionPanel c;
    private ConstructionType selectedType;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        c = (ConstructionPanel)target;

        selectedType = (ConstructionType)EditorGUILayout.EnumPopup(selectedType);
        if (GUILayout.Button("Show/Hide Items"))
        {
            c.ShowHideAllItemsOfType(selectedType);
        }

    }

    private void OnEnable() => c.constructionItems = FindObjectsOfType(typeof(ConstructionItem)) as ConstructionItem[];

}
