using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;



public class JsonConstructor : MonoBehaviour
{
    //List<GameObject> gos= new List<GameObject>();
    const string _path = "Assets/Resources/JsonConstructor/";
    List<string> exceptions = new List<string> { "Tipos de tubería PVC PIPE SN4 DN=CHECK COMPARATIVE TABLE FOR PVC Ø/DN [627201]" };
    private List<GameObject> gos;

    public void InstantiateGOsFromFile(string path, GameObject prefab)
    {

        Debug.Log(path);
        var jsonStringFile = File.ReadAllText(path);
        var itemPositions = JsonConvert.DeserializeObject<BaseCircuitComponets>(jsonStringFile);

        foreach (var posAndRot in itemPositions.posAndRot)
        {
            var pos = posAndRot.position;
            var rot = posAndRot.rotation;
            GameObject go = Instantiate(prefab, pos.ToVector3(), rot.ToQuaternion());
            go.SetActive(true);
        }
    }




    public void InstantiateAllGOs() {

        string myPathFiles = "Assets/Resources/JsonConstructor/";
        DirectoryInfo dir = new DirectoryInfo(myPathFiles);
        FileInfo[] info = dir.GetFiles("*.*");

        foreach (FileInfo f in info)
        {
            string prefabName = f.Name.Replace(".txt", "");
            Debug.Log("prefabName = " + prefabName);
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GreenHouse/" + prefabName);

            if (prefab != null)
            {
                InstantiateGOsFromFile(_path + prefabName + ".txt", prefab);
            }
            else Debug.Log("Prefab es null");

        }
    }

    public void SaveAllGOs()
    {
        Dictionary<string, BaseCircuitComponets> gosPosAndRot = new Dictionary<string, BaseCircuitComponets>();
        gos = GameObject.FindGameObjectsWithTag("Item").ToList();
        foreach (GameObject go in gos)
        {
            string nameGO = "";
            if (go.name.Contains("DK-12170 DK-12097 DK-12170 DK-12097") || go.name.Contains("PD-83731 PD-83731 "))            
            {
                nameGO = go.name.Replace('Ø', '_').Replace('/', '_').Replace(' ', '_');
            }
            else
            {
                 nameGO = go.name.Split('[')[0].Replace('Ø', '_').Replace('/', '_').Replace(' ', '_');
            }
            
            string localPath = "Assets/Resources/Prefabs/GreenHouse/" + nameGO + ".prefab";
            if (!File.Exists(localPath)) {//Si no existe el prefab lo guardo
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, localPath);
            }
            if (!gosPosAndRot.ContainsKey(nameGO))
            {
                gosPosAndRot.Add(nameGO, null);
            }
            if (gosPosAndRot[nameGO] == null)
            {
                gosPosAndRot[nameGO] = new BaseCircuitComponets();
            }
            Debug.Log(gosPosAndRot[nameGO]);
            gosPosAndRot[nameGO].AddPosAndRot(go.transform.position, go.transform.rotation);
        }
        foreach (var goPosAndRot in gosPosAndRot)
        {
            var key = goPosAndRot.Key;
            var posAndRot = goPosAndRot.Value;
            string json = JsonConvert.SerializeObject(posAndRot);

            string path = "Assets/Resources/JsonConstructor/" + key + ".txt";
            Debug.Log(path);
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(json);
            writer.Close();
        }
    }

    /*
    public void Replace()
    {
        Dictionary<string, BaseCircuitComponets> gosPosAndRot = new Dictionary<string, BaseCircuitComponets>();
        gos = GameObject.FindGameObjectsWithTag("Item").ToList();
        foreach (GameObject go in gos)
        {
            string nameGO = go.name.Split('[')[0].Replace('Ø', '_').Replace('/', '_').Replace(' ', '_');
            string localPath = "Assets/Resources/Prefabs/GreenHouse/" + nameGO + ".prefab";
            if (!File.Exists(localPath))
            {//Si no existe el prefab lo guardo
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, localPath);
            }
            if (!gosPosAndRot.ContainsKey(nameGO))
            {
                gosPosAndRot.Add(nameGO, null);
            }
            if (gosPosAndRot[nameGO] == null)
            {
                gosPosAndRot[nameGO] = new BaseCircuitComponets();
            }
            Debug.Log(gosPosAndRot[nameGO]);
            gosPosAndRot[nameGO].AddPosAndRot(go.transform.position, go.transform.rotation);
        }
        foreach (var goPosAndRot in gosPosAndRot)
        {
            var key = goPosAndRot.Key;
            var posAndRot = goPosAndRot.Value;
            string json = JsonConvert.SerializeObject(posAndRot);

            string path = "Assets/Resources/JsonConstructor/" + key + ".txt";
            Debug.Log(path);
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(json);
            writer.Close();
        }
    }*/
}
