using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssignMaterials : EditorWindow
{
    public GameObject objectToAssingMaterial;
    public Dictionary<string, List<string>> modelMatDictionary = new Dictionary<string, List<string>>();
   
    List<string> tempMaterialList = new List<string>();
    string modelName;
    string filePathing = "";    

    [MenuItem("Window/AssignMaterial")]
    static void Init()
    {
        AssignMaterials window = (AssignMaterials)EditorWindow.GetWindow(typeof(AssignMaterials));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Assign Materials", EditorStyles.boldLabel);
        filePathing = EditorGUILayout.TextField("File Path", filePathing);
        if (GUILayout.Button("Assign Materials") && Directory.Exists(Application.dataPath + filePathing))
        {
            ReadAndAdd(filePathing);
        }
    }

    void ReadAndAdd(string filePath)
    {
        filePath = Application.dataPath + filePath;
        StreamReader strFile = new StreamReader(filePath);
        tempMaterialList.Clear();
        while (!strFile.EndOfStream)
        {
            string actualLine = strFile.ReadLine();
            string[] splittedLine = actualLine.Split("|");

            foreach (char s in splittedLine[0])
            {
                if(s != '"')
                    modelName += s;
            }

            string[] materialSplittedStr = splittedLine[1].Split(',');
            for (int i = 0; i < materialSplittedStr.Length; i++)
            {
                string material = "";
                foreach (char s in materialSplittedStr[i])
                {
                    if (s != '|' && s != '"')
                        material += s;
                }
                tempMaterialList.Add(material);
            }
            modelMatDictionary.Add(modelName, tempMaterialList);
        }
        strFile.Close();
        LoadMesh();
    }

    void LoadMesh()
    {
        GameObject[] submeshArray = Resources.LoadAll<GameObject>("GreenhouseModel/");
        foreach (var submesh in submeshArray)
        {
            if (modelMatDictionary.ContainsKey(submesh.name))
            {
                LinkMaterials(submesh);
            }
        }
    }

    void LinkMaterials(GameObject mesh)
    {
        List<string> materialList = new List<string>();
        modelMatDictionary.TryGetValue(mesh.name, out materialList);
        Material[] matArray = Resources.LoadAll<Material>("Materials/");
        MeshRenderer meshRenderer = mesh.GetComponent<MeshRenderer>();
        int i = 0;
        int j = 0;
        while (i < materialList.Count && j < matArray.Length)
        {
            Material mat = matArray[j];
            if (materialList.Contains(mat.name))
            {
                meshRenderer.materials[i] = mat;
                i++;
            }
            j++;
        }       
    }

    
}
