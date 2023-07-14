using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BindToModel : Editor
{        
    public static Material[] GetMaterials(string path)
    {
        return Resources.LoadAll<Material>(path);
    }

    public static void GetModels(string path, string materialPath)
    {
        try
        {
            if (File.Exists(path))
            {
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                List<ModelClass> models = new List<ModelClass>();
                models = JsonConvert.DeserializeObject<List<ModelClass>>(json);
                BindModelMaterials(models, materialPath);
            }
        }
        catch (Exception e) { Debug.Log(e.Message); }
    }

    public static void BindModelMaterials(List<ModelClass> models, string materialPath)
    {
        Material[] materials = GetMaterials(materialPath);
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject gb in sceneObjects)
        {
            if (gb.name.Contains("{3D}")) DestroyImmediate(gb);
            else
            {
                ModelClass actualModel = GameObjectInModel(models, gb);
                if (actualModel != null && gb.GetComponent<MeshRenderer>() != null)
                    SearchAndBind(actualModel, gb, materials);
            }
        }
    }

    static ModelClass GameObjectInModel(List<ModelClass> models, GameObject gb)
    {
        ModelClass model = null;
        string name = gb.name;
        string id = "";
        if (gb.name.Contains("[") && gb.name.Contains("]"))
        {
            id = gb.name.Split("[", StringSplitOptions.None)[1].Split("]", StringSplitOptions.None)[0];
            foreach (ModelClass mod in models)
            {
                if (mod.Id == id)
                {
                    model = mod;
                    break;
                }
            }
        }
        return model;
    }

    static void SearchAndBind(ModelClass model, GameObject gb, Material[] materials)
    {
        List<Material> newGbMaterials = new List<Material>();
        foreach(string name in model.materials)
        {
            foreach(Material actualMaterial in materials)
            {
                if(name == actualMaterial.name)
                {
                    newGbMaterials.Add(actualMaterial);
                    break;
                }
            }
        }
        gb.GetComponent<MeshRenderer>().materials = newGbMaterials.ToArray(); 
    }
    
}
