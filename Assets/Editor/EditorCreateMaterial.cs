
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EditorCreateMaterial : EditorWindow
{
    string folderPath;
    bool finished;
    [MenuItem("Tools/Import and Bind")]
    public static void ShowWindow()
    {
        EditorCreateMaterial window = GetWindow<EditorCreateMaterial>();
        window.titleContent = new GUIContent("Import and Bind");
        window.minSize = new Vector2(100, 100);
    }
       

    private void OnGUI()
    {
        GUILayout.Label("", EditorStyles.boldLabel);
        GUILayout.Label("Select and import the structure and the materials", EditorStyles.boldLabel);
        GUILayout.Label("", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Folder"))
        {
            folderPath = EditorUtility.OpenFolderPanel("Select StructureFBX.fbx folder", "Assets/", "json");            
        }
        EditorGUILayout.TextField("Path", folderPath);
        GUILayout.Label("", EditorStyles.boldLabel);
        if (GUILayout.Button("Create materials and import structure"))
        {
            try 
            {
                string materialPath = folderPath + @"\Materials.json";
                if (File.Exists(materialPath)) CreateMaterials.LoadJson(materialPath);

                string fbxPath = folderPath + @"\StructureFBX.fbx";

                if (!Directory.Exists(Application.dataPath + @"\Structure"))
                {
                    Directory.CreateDirectory(Application.dataPath + @"\Structure");
                }

                File.Copy(fbxPath, Application.dataPath + @"\Structure\StructureFBX.fbx");
                AssetDatabase.Refresh();
                GameObject gb = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Structure/StructureFBX.fbx");   
                
                GameObject instance = Instantiate(gb, gb.transform.position, gb.transform.rotation);
                string structurePath = folderPath + @"\Structure.json";

                if (File.Exists(structurePath)) 
                {
                    BindToModel.GetModels(structurePath, "Materials");                  
                    if (!Directory.Exists(Application.dataPath + @"\Prefabs"))
                    {
                        Directory.CreateDirectory(Application.dataPath + @"\Prefabs");
                    }
                    string localPath = "Assets/Prefabs/" + gb.name + ".prefab";
                    bool prefabSuccess = false;
                    GameObject prefab=PrefabUtility.SaveAsPrefabAssetAndConnect(instance, localPath, InteractionMode.UserAction, out prefabSuccess);
                    DestroyImmediate(instance);
                    string sceneName = "Structure";
                    int index = 0;
                    string charIndex = "";
                    string aux = sceneName;
                    for (int i = 0; i < SceneManager.sceneCount; i++)
                    {
                        if (SceneManager.GetSceneAt(i).name == aux)
                        {
                            index += 1;
                            charIndex = index.ToString();
                            aux = sceneName + charIndex;
                        }
                    }
                    sceneName = aux;
                    Scene scene = CreateProjectScene(sceneName);
                    ProjectManagerDemo projectManagerDemo = FindObjectOfType<ProjectManagerDemo>();
                    projectManagerDemo.sceneName = sceneName;
                    EditorBuildSettingsScene sceneBuild = new EditorBuildSettingsScene(scene.path, true);
                    EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length+1];
                    for(int i=0; i<buildScenes.Length; i++)
                    {
                        if (i == buildScenes.Length - 1) buildScenes[i] = sceneBuild;
                        else buildScenes[i] = EditorBuildSettings.scenes[i];
                    }
                    EditorBuildSettings.scenes = buildScenes;                    
                    PrefabUtility.InstantiatePrefab(prefab, scene);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                    EditorSceneManager.SaveScene(scene);
                    EditorSceneManager.CloseScene(scene, true);
                }

            } 
            catch(Exception e) { EditorUtility.DisplayDialog("Something went wrong", e.Message + " "+ e.StackTrace, "OK"); }            
        }         
    }

    Scene CreateProjectScene(string name)
    {
        Scene scene= SceneManager.GetSceneByPath("Assets/Scenes/" + name + ".unity");
        if (!scene.IsValid())
        {
            scene = EditorSceneManager.NewScene(0, NewSceneMode.Additive);
        }       
        scene.name = name;
        
        if (!Directory.Exists(Application.dataPath + @"\Scenes"))
        {
            Directory.CreateDirectory(Application.dataPath + @"\Scenes");
        }
        string scenePath = "Assets/Scenes/" + scene.name + ".unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        return scene;
    }
    
}
