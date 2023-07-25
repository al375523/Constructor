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
                {
                    //SearchAndBind(actualModel, gb, materials);
                    SearchAndBindSingular(actualModel, gb, materials, materialPath);
                }
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

    static void SearchAndBindSingular(ModelClass model, GameObject gb, Material[] materials, string materialPath)
    {
        string shaderName = "Universal Render Pipeline/Lit";
        Material matCombination = new Material(Shader.Find(shaderName));

        float glossiness = 0f;
        float metallic = 0f;
        Color c = new Color(0f, 0f, 0f, 0f);

        bool transparency = false;
        bool texAdded = false;
        float glassAlpha = -1f;

        string cutName = "";
        foreach (char charac in gb.name)
        {
            if (charac.ToString() == "[")
                break;
            if (charac.ToString() != " " && charac.ToString() != "/" && charac.ToString() != @"\")
                cutName += charac;
        }
        Material createdMaterial = null;
        try
        {
            createdMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/Materials/" + cutName + ".mat");
        }
        catch
        {

        }
        Debug.Log("Continue");
        if (createdMaterial == null)
        {
            Debug.Log("False");
            foreach (string name in model.materials)
            {
                foreach (Material actualMaterial in materials)
                {
                    if (name == actualMaterial.name)
                    {
                        if (!transparency && actualMaterial.GetFloat("_Surface") == 1.0f)
                        {
                            matCombination.SetFloat("_Surface", 1.0f);
                            transparency = true;
                            c.a = actualMaterial.color.a;
                        }

                        if (!texAdded && actualMaterial.mainTexture != null)
                        {
                            matCombination.mainTexture = actualMaterial.mainTexture;
                            texAdded = true;
                        }

                        c = new Color(c.r + actualMaterial.color.r, c.g + actualMaterial.color.g, c.b + actualMaterial.color.b, c.a + actualMaterial.color.a);
                        glossiness += actualMaterial.GetFloat("_Glossiness");
                        metallic += actualMaterial.GetFloat("_Metallic");

                        break;
                    }
                }
            }
            c = new Color(c.r / model.materials.Count, c.g / model.materials.Count, c.b / model.materials.Count, c.a / model.materials.Count);
            if (glassAlpha != -1) c.a = glassAlpha;
            glossiness /= model.materials.Count;
            metallic /= model.materials.Count;

            matCombination.color = c;
            matCombination.SetFloat("_Glossiness", glossiness);
            matCombination.SetFloat("_Metallic", metallic);

            AssetDatabase.CreateAsset(matCombination, "Assets/Resources/Materials/" + cutName + ".mat");
            AssetDatabase.Refresh();
        }
        else
        {
            matCombination = createdMaterial;
        }
        List<Material> materialList = new List<Material>();
        materialList.Add(matCombination);

        gb.GetComponent<MeshRenderer>().materials = materialList.ToArray();
        if (gb.name.Contains("Surface") || gb.name.Contains("Pad Pad")) DestroyImmediate(gb);
    }

}
