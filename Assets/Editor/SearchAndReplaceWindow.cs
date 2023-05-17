using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class SearchAndReplaceWindow : EditorWindow
{
    public SearchAndReplace searchAndReplace;
    private GUIStyle _titleStyle;
    bool shouldShowNameFound;
    Vector2 scrollPos = new Vector2();
    [MenuItem("Window/SearchAndReplace")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SearchAndReplaceWindow window = (SearchAndReplaceWindow)EditorWindow.GetWindow(typeof(SearchAndReplaceWindow));
        window.searchAndReplace = new SearchAndReplace();
        window.searchAndReplace.Clean();
        window.Show();
    }

    private void OnGUI()
    {
        _titleStyle = new GUIStyle();
        _titleStyle.fontStyle = FontStyle.Bold;
        _titleStyle.alignment = TextAnchor.MiddleCenter;


        EditorGUILayout.LabelField("Search and Replace", _titleStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name to search: ");
        searchAndReplace.wordToSearch = EditorGUILayout.TextField(searchAndReplace.wordToSearch);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        /*if (GUILayout.Button("Search"))
        {
            searchAndReplace.SearchGOsThatStartWith(searchAndReplace.wordToSearch);

        }*/
        if (GUILayout.Button("Search with Regex"))
        {
            Debug.Log("busco" );
            searchAndReplace.gosFound=searchAndReplace.SearchGOsRegex(searchAndReplace.wordToSearch);
            Debug.Log("encontre:"+searchAndReplace.gosFound);

        }
        EditorGUILayout.EndHorizontal();
        if (searchAndReplace.gosFound.Count > 0)
        {
            EditorGUILayout.HelpBox(searchAndReplace.gosFound.Count + " matches found", MessageType.Info);
            if (GUILayout.Button("Show Names Found"))
            {
                shouldShowNameFound = true;
            }
            if (shouldShowNameFound)
            {

                var names = searchAndReplace.GetNamesSearched(searchAndReplace.gosFound);
                
                scrollPos =EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(500));
                foreach (var name in names)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(name);
                    if (GUILayout.Button("Select"))
                    {
                        //searchAndReplace.SearchGOsRegex(searchAndReplace.wordToSearch);
                        Selection.SetActiveObjectWithContext(searchAndReplace.GetGo(searchAndReplace.gosFound, name),null);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            Separation();
            EditorGUILayout.LabelField("Replace with: ");
            searchAndReplace.goToReplace = (GameObject)EditorGUILayout.ObjectField(searchAndReplace.goToReplace, typeof(GameObject));
            if (searchAndReplace.gosFound.Count > 0 && searchAndReplace.goToReplace != null && GUILayout.Button("Replace"))
            {
                searchAndReplace.ReplaceCurrentSearch(true, true,true);
                searchAndReplace.Clean();
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
