using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DeleteObjectsWindow : EditorWindow
{
    public DeleteObjects deleteObjects;
    private GUIStyle _titleStyle;
    private List<string> searchedGONames;
    bool shouldShowNameFound;
    bool gosAreReplaced;
    Vector2 scrollPos = new Vector2();
    [MenuItem("Window/DeleteObjects")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DeleteObjectsWindow window = (DeleteObjectsWindow)EditorWindow.GetWindow(typeof(DeleteObjectsWindow));
        window.deleteObjects = new DeleteObjects();
        window.deleteObjects.Clean();
        window.Show();
    }

    private void OnGUI()
    {
        _titleStyle = new GUIStyle();
        _titleStyle.fontStyle = FontStyle.Bold;
        _titleStyle.alignment = TextAnchor.MiddleCenter;


        EditorGUILayout.LabelField("Delete Defaults Objects", _titleStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox("Going to delete:" + deleteObjects.GetNameDefaultObjectsToDelete(), MessageType.Info);
        if (GUILayout.Button("Delete"))
        {
            deleteObjects.DeleteDefaultObjects();
        }


        EditorGUILayout.EndHorizontal();
        Separation();
        EditorGUILayout.LabelField("DeleteObjects", _titleStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name to search: ");
        deleteObjects.wordToSearch = EditorGUILayout.TextField(deleteObjects.wordToSearch);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Search with Regex"))
        {
            deleteObjects.gosFound=deleteObjects.SearchGOsRegex(deleteObjects.wordToSearch);

        }
        EditorGUILayout.EndHorizontal();
        if (deleteObjects.gosFound.Count > 0)
        {
            EditorGUILayout.HelpBox(deleteObjects.gosFound.Count + " matches found", MessageType.Info);
            if (GUILayout.Button("Show Names Found"))
            {
                shouldShowNameFound = true;
            }
            if (shouldShowNameFound)
            {

                var names = deleteObjects.GetNamesSearched();

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(500));
                foreach (var name in names)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(name);
                    if (GUILayout.Button("Select"))
                    {
                        //deleteObjects.SearchGOsRegex(deleteObjects.wordToSearch);
                        Selection.SetActiveObjectWithContext(deleteObjects.GetGo(deleteObjects.gosFound ,name), null);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            Separation();
            if (deleteObjects.gosFound.Count > 0 && GUILayout.Button("Delete"))
            {
                deleteObjects.DeleteFoundedObjects();
                deleteObjects.Clean();
            }
        }
    }

    private static void Separation()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        var rect = EditorGUILayout.GetControlRect(false, 1f);
        rect.height = 1f;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
