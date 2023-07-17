using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateMaterials : Editor
{
    public string materialPath;
    public List<Material> materialList;
  
    public static void LoadJson(string path)
    {
        Debug.Log(path);
        try
        {           
            if (File.Exists(path))
            {
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                List<MaterialProject> materialList = JsonConvert.DeserializeObject<List<MaterialProject>>(json);
                CreateNewMaterials(materialList);                        
            }            
        }catch(Exception e) { Debug.Log(e.Message+" Stack: "+e.StackTrace); }
    }

    public static void CreateNewMaterials(List<MaterialProject> materialList)
    {
        if (!Directory.Exists("Assets/Resources"))
        {
            Directory.CreateDirectory("Assets/Resources");
        }
        //Material Folder Path
        string materialFolderPath = "Assets/Resources/Materials/";
        if (!Directory.Exists(materialFolderPath))
        {
            Directory.CreateDirectory(materialFolderPath);
        }

        //Texture Folder Path
        string textureFolderPath = "Assets/Resources/Textures/";
        if (!Directory.Exists(textureFolderPath))
        {
            Directory.CreateDirectory(textureFolderPath);
        }

        string shaderName = "";
        
        shaderName = "Universal Render Pipeline/Lit";
        
        foreach (MaterialProject mat in materialList)
        {           
            Material aux = new Material(Shader.Find(shaderName));
            
            aux.name = mat.name;
            aux.SetFloat("_Glossiness", 1 - mat.shininess / 128);
            aux.SetFloat("_Metallic", 1 - mat.smoothness / 50);

            if (mat.texturesPath.Length > 0)
            {
                aux.mainTexture = CreateTextureAsset(mat, textureFolderPath);
            }
            Color color = new Color();
            string[] splittedColor = mat.color.Split(" ", System.StringSplitOptions.None);
            foreach (string str in splittedColor)
            {
                string[] auxSplitted = str.Split(":", System.StringSplitOptions.None);
                if (auxSplitted[0] == "R")      { color.r = int.Parse(auxSplitted[1]) / 255f; }
                else if (auxSplitted[0] == "G") { color.g = int.Parse(auxSplitted[1]) / 255f; }
                else if (auxSplitted[0] == "B") { color.b = int.Parse(auxSplitted[1]) / 255f; }
            }
            if (mat.glow > 0f) 
            {
                aux.EnableKeyword("_EMISSION");
                aux.SetColor("_EmissionColor", new Color(1f, 1f, 1f, mat.glow));                
            }
            color.a = 1f - (mat.transparency / 100f);
            aux.color = color;
            if (mat.transparency > 0)
            {
                aux.SetFloat("_surface", 1);
            }                     
            AssetDatabase.CreateAsset(aux,  materialFolderPath + @aux.name + ".mat");
            AssetDatabase.Refresh();

        }
        //BindToModel.BindModelMaterials(modelList, materialFolderPath);
    }

    static Texture2D CreateTextureAsset(MaterialProject material, string textureFolderPath)
    {
        string path = material.texturesPath[0];
        string extension = "";
        if (path.Contains("png")) extension = ".png";
        else extension = ".jpg";
        if (File.Exists(path))
        {            
            FileUtil.CopyFileOrDirectory(path, textureFolderPath + @material.name + "Tex"+extension);           
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Texture2D tex= AssetDatabase.LoadAssetAtPath<Texture2D>(textureFolderPath + @material.name + "Tex" + extension);
            return tex;
        }
        return null;        
    }


    
}
