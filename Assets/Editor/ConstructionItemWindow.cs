using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConstructionItemWindow : EditorWindow
{
    public ConstructionItem[] constructionItems;
    ConstructorUtility constructorUtility;
    private GUIStyle _titleStyle;
    bool shouldShowNameFound;
    Dictionary<string, ConstructionType> defaultObjectsTypes = new Dictionary<string, ConstructionType>()
        {
            { "DK-", ConstructionType.Ceiling},
            { "83700", ConstructionType.Ceiling},
            { "Barra", ConstructionType.Ceiling},
            { "glass", ConstructionType.Ceiling},
            { "GA-01208", ConstructionType.Ceiling},
            { "gutter", ConstructionType.Structure},
            { "OK-", ConstructionType.Structure},
            { "PD-", ConstructionType.Structure},
            { "SK-", ConstructionType.Structure},
            { "TURNBUNKLE", ConstructionType.Structure},
            { "SoporteVentana", ConstructionType.Structure},
            { "supporto", ConstructionType.Equipment},
            { "Tipos", ConstructionType.Equipment},
            { "Accessori", ConstructionType.Equipment},
            { "M_Gomito", ConstructionType.Equipment},
            { "Aislamiento", ConstructionType.Equipment}
        };
    private Vector2 scrollPos;
    private List<GameObject> gosFounded;
    private ConstructionType selectedType;

    [MenuItem("Window/ConstructionItems")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ConstructionItemWindow window = (ConstructionItemWindow)EditorWindow.GetWindow(typeof(ConstructionItemWindow));
        window.constructorUtility = new ConstructorUtility();
        window.gosFounded = new List<GameObject>();
        window.Show();
    }
    private void OnGUI()
    {
        _titleStyle = new GUIStyle();
        _titleStyle.fontStyle = FontStyle.Bold;
        _titleStyle.alignment = TextAnchor.MiddleCenter;

        constructionItems = FindObjectsOfType<ConstructionItem>();


        EditorGUILayout.LabelField("Construction Items Assigments", _titleStyle);
        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.HelpBox(":" + constructionItem.GetNameDefaultObjectsToDelete(), MessageType.Info);
        if (GUILayout.Button("Apply"))
        {
            foreach (var objectNameAndType in defaultObjectsTypes)
            {
                var name = objectNameAndType.Key;
                var type = objectNameAndType.Value;
                var gos = constructorUtility.SearchGOsRegex(name);

                ApplyTypeToGOs(type, gos);
            }
        }


        EditorGUILayout.EndHorizontal();
        Separation();
        EditorGUILayout.LabelField("Construction Items :", _titleStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name to search: ");
        constructorUtility.wordToSearch = EditorGUILayout.TextField(constructorUtility.wordToSearch);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Search with Regex"))
        {
            Debug.Log("busco con regex");
             gosFounded =constructorUtility.SearchGOsRegex(constructorUtility.wordToSearch);

        }
        EditorGUILayout.EndHorizontal();
        if (gosFounded.Count > 0)
        {
            EditorGUILayout.HelpBox(gosFounded.Count + " matches found", MessageType.Info);
            if (GUILayout.Button("Show Names Found"))
            {
                shouldShowNameFound = true;
            }
            if (shouldShowNameFound)
            {
                
                var names = constructorUtility.GetNamesSearched(gosFounded);

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(500));
                foreach (var name in names)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(name);
                    if (GUILayout.Button("Select"))
                    {
                        gosFounded=constructorUtility.SearchGOsRegex(constructorUtility.wordToSearch);
                        Selection.SetActiveObjectWithContext(constructorUtility.GetGo(gosFounded, name), null);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
                
            }
            Separation();
            selectedType = (ConstructionType)EditorGUILayout.EnumPopup(selectedType);
            if (gosFounded.Count > 0 && GUILayout.Button("Apply Type"))
            {
                ApplyTypeToGOs(selectedType, gosFounded);
            }
        }

        
    }

    private void ApplyTypeToGOs(ConstructionType type, List<GameObject> gos)
    {
        foreach (var item in constructionItems)
        {
            if (type == item.type)
            {
                foreach (var go in gos)
                {
                    item.SetChildren(go);
                }
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
