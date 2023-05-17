using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonReader))]
public class JsonReaderEditor : Editor
{
    public int selected = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        JsonReader jsonReader = (JsonReader) target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Json File"))
        {
            jsonReader.LoadJsonFile("Assets/Resources/Text/Circuit/" + jsonReader.fileName);
        }

        if (GUILayout.Button("Save Json File"))
        {
            jsonReader.SaveToJson();
        }

        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Load all Json Files"))
        {
            jsonReader.LoadAllCircuitJsonFiles();
        }

        if (jsonReader.jsonFiles.Count > 0)
        {
            EditorGUI.BeginChangeCheck();
        /*    selected = EditorGUILayout.Popup("Select file to instantiate", jsonReader.selected,
                jsonReader.optionsPopUp);
            if (EditorGUI.EndChangeCheck())
            {
                jsonReader.LoadJsonFile("Assets/Resources/Text/Circuit/" + jsonReader.optionsPopUp[selected] + ".json");
            }*/
        }
    }
}