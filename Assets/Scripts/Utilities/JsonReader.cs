using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class JsonReader : MonoBehaviour
{
    [Header("Files")] [HideInInspector] public string finalPath;
    public string fileName = "CircuitData.json";
    [SerializeField] public int _numberFile = 1;
    [SerializeField] public int maxNumberFile;
    private string JSON_PATH = "Assets/Resources/Text/";
    public List<TextAsset> jsonFiles = new List<TextAsset>();
    //public List<string> jsonFilesNames = new List<string>();
    [Header("Instances from Json")] public GreenHouse _greenHouse;
    public CircuitItems _circuit;

    [Header("References")] 
    public CircuitManager circuitManager;

    public bool saveToNew = true;
    [HideInInspector] public GameObject[] parentPrefabs;

    //  GameObject itemInstantiated;
    public Dictionary<string, string> IDToPrefabName = new Dictionary<string, string>();

    public List<CircuitComponents> _componentsArrays = new List<CircuitComponents>();

    private string jsonStringFile;
    private string path;
    [NonSerialized] public int selected = 0;
    public DropdownArrow dropdown;
    private GameObject player;
    private string tempData;

    const byte ShareCircuitPlayer = 1;
    private const string fullpath = "Assets/Resources/Text/Circuit/CircuitData";
    private const string circuitpath = "Assets/Resources/Text/Circuit/";
    private void Awake()
    {
        finalPath = JSON_PATH + fileName;
        circuitManager = FindObjectOfType<CircuitManager>();
        LoadAllCircuitJsonFiles();

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //StartCoroutine(WaitDropdown());
    }
    public void Set(DropdownArrow newDropdown) {
        if (dropdown == null) {
            dropdown = newDropdown;
            _numberFile = dropdown.dropdown.value;
            dropdown.Set(jsonFiles.Select(x => x.name));
        }

    }

    private void AddToComponentArray()
    {
        _componentsArrays.Clear();
        if (_circuit.WaterTanks != null)
        {
            _circuit.WaterTanks.Sort();
            _componentsArrays.AddRange(_circuit.WaterTanks);
        }

        if (_circuit.Boilers != null)
        {
            _circuit.Boilers.Sort();
            _componentsArrays.AddRange(_circuit.Boilers);
        }

        if (_circuit.Pipes != null)
        {
            _circuit.Pipes.Sort();
            _componentsArrays.AddRange(_circuit.Pipes);
        }


        if (_circuit.Elbows != null)
        {
            _circuit.Elbows.Sort();
            _componentsArrays.AddRange(_circuit.Elbows);
        }


        if (_circuit.Reductors != null)
        {
            _circuit.Reductors.Sort();
            _componentsArrays.AddRange(_circuit.Reductors);
        }


        if (_circuit.Valves != null)
        {
            _circuit.Valves.Sort();
            _componentsArrays.AddRange(_circuit.Valves);
        }

        _componentsArrays.Sort();
    }

    public void LoadJsonFile(string path)
    {
        if (circuitManager != null) circuitManager.ResetCircuit();
        jsonStringFile = File.ReadAllText(path);
        _numberFile = dropdown.dropdown.value;
        SaveInstancesFromJson();
        InstantiateItems();
        RemodelItems();
    }


    public void LoadJsonFile(CircuitItems circuit)
    {
        if (circuitManager != null) circuitManager.ResetCircuit();
        _circuit = circuit;  
        AddToComponentArray();
        InstantiateItems();
        RemodelItems();
    }

    public void LoadTempFile(string tempData)
    {
        if (circuitManager != null) circuitManager.ResetCircuit();
        jsonStringFile = tempData;
        SaveInstancesFromJson();
        InstantiateItems();
        RemodelItems();
    }
    public void RemodelItems()
    {
        circuitManager.circuit.ChangeModels(_componentsArrays);
    }

    public void SaveInstancesFromJson()
    {
        //_greenHouse = JsonUtility.FromJson<GreenHouse>(jsonString);

        _circuit = JsonUtility.FromJson<CircuitItems>(jsonStringFile);
        AddToComponentArray();
    }

    public void SearchAndAssignParent(string searchPrefab, GameObject itemInstantiated)
    {
        Transform parentObject = null;
        parentObject = searchPrefab switch
        {
            "CropGutters" => parentPrefabs[0].transform,
            "Coordinates Vertical Grid" => parentPrefabs[1].transform,
            "Walls" => parentPrefabs[2].transform,
            "Lamps" => parentPrefabs[3].transform,
            "Fans" => parentPrefabs[4].transform,
            "Doors" => parentPrefabs[5].transform,
            "Tanks" => parentPrefabs[6].transform,
            _ => parentObject
        };

        itemInstantiated.transform.SetParent(parentObject);
    }

    public void InstantiateItems()
    {
        for (int i = 0; i < _componentsArrays.Count; i++)
        {
            circuitManager.InstantiateItem(_componentsArrays[i].ID);
        }
        circuitManager.SelectLastItem();
        circuitManager.SetAllPrevAndNextCircuitItems();
    }

 public GameObject LoadPrefab(string folder, string prefSearch)
    {
        return Resources.Load<GameObject>("Prefabs/" + folder + prefSearch);
    }

    public void SaveToJson()
    {
        _numberFile = dropdown.dropdown.value;
        if (_circuit == null) return;
        if (_greenHouse == null) return;
        _circuit.ClearCircuit();
        SaveToCircuitList();
        string circuitJson = JsonUtility.ToJson(_circuit);
        path = fullpath + _numberFile + ".json";
        if (saveToNew)
        {
            while (File.Exists(path))
            {
                _numberFile++;
                path = fullpath + _numberFile + ".json";
                Debug.Log("saving");
                LoadAllCircuitJsonFiles();
                dropdown.AddNewOption(path);
            }
            File.WriteAllText(path, circuitJson);
        }
        else if(jsonFiles!=null)
        {
            if (Application.isPlaying) path = circuitpath + jsonFiles[_numberFile].text + ".json";
            if (Application.isEditor) path = fullpath + _numberFile + ".json";
            File.WriteAllText(path, circuitJson);
        }
        ResetCircuit();
    }

    public void SaveToJson(string fileName, string content)
    {

        File.WriteAllText(circuitpath+ fileName, content);
 
    }


    public void SaveToJsonTemp()
    {
        if (_circuit == null) return;
        if (_greenHouse == null) return;
        _circuit.ClearCircuit();
        SaveToCircuitList();
        tempData = JsonUtility.ToJson(_circuit);
        path = circuitpath  +"temp.json";
        File.WriteAllText(path, tempData);

    }


    private void ResetCircuit()
    {
        _circuit = new CircuitItems(new List<WaterTankItem>(), new List<BoilerItem>(), new List<ElbowItem>(), new List<PipeItem>(),
            new List<ReductorItem>(), new List<ValveItem>(),"27/07/2022","",0,"");
    }

    public void SaveToCircuitList()
    {
        CircuitItem[] itemCircuits = FindObjectsOfType<CircuitItem>();
        CreateInstances(itemCircuits);
    }

    private void CreateInstances(CircuitItem[] itemCircuits)
    {
        string ID;
        int positionInCircuit;
        Vector3 itemPosition;
        Position[] position;
        Rotation rotation;
        string modelo;
        for (int i = 0; i < itemCircuits.Length; i++)
        {
            //Common Attributes
            ID = itemCircuits[i].ID;
            positionInCircuit = itemCircuits[i].positionInCircuit;
            itemPosition = itemCircuits[i].gameObject.transform.position;
            position = new[] {new Position(itemPosition.x, itemPosition.y, itemPosition.z)};
            Quaternion itemRot = itemCircuits[i].transform.rotation;
            modelo = itemCircuits[i].currentModel;

            rotation = new Rotation(itemRot.eulerAngles.x, itemRot.eulerAngles.y, itemRot.eulerAngles.z);

            if (itemCircuits[i].ID.Contains("Water Tank"))
            {
                modelo = itemCircuits[i].gameObject.transform.GetChild(0).gameObject.name;
                _circuit.WaterTanks.Add(new WaterTankItem(ID, positionInCircuit, position, rotation, modelo));
            }
            if (itemCircuits[i].ID.Contains("Boiler"))
            {
                modelo = itemCircuits[i].gameObject.transform.GetChild(0).gameObject.name;
                _circuit.Boilers.Add(new BoilerItem(ID, positionInCircuit, position, rotation, modelo));
            }
            if (itemCircuits[i].ID.Contains("Pipe"))
            {
                Measurements measures;
                position = new[]
                {
                    new Position(itemCircuits[i].initialNormal.position.x, itemCircuits[i].initialNormal.position.y,
                        itemCircuits[i].initialNormal.position.z),
                    new Position(itemCircuits[i].endNormal.position.x, itemCircuits[i].endNormal.position.y,
                        itemCircuits[i].endNormal.position.z)
                };
                measures = itemCircuits[i].gameObject.GetComponentInChildren<Measurements>();
                float width = measures.width1;
                float height = measures.heightMeasurement;
                _circuit.Pipes.Add(new PipeItem(ID, positionInCircuit, position, rotation, width, height, modelo));
            }

            if (itemCircuits[i].ID.Contains("Elbow"))
            {
                var measures = itemCircuits[i].gameObject.GetComponentInChildren<Measurements>();
                float width = measures.width1;
                _circuit.Elbows.Add(new ElbowItem(ID, positionInCircuit, position, rotation, modelo, width));
            }

            if (itemCircuits[i].ID.Contains("Reductor"))
            {
                float salidaJSON = itemCircuits[i].gameObject.GetComponent<Reductor>().salidaJSON;
                var measures = itemCircuits[i].gameObject.GetComponentInChildren<Measurements>();
                float width = measures.width1;
                _circuit.Reductors.Add(new ReductorItem(ID, positionInCircuit, position, rotation, salidaJSON, modelo, width));
            }

            if (itemCircuits[i].ID.Contains("Valve"))
            {
                bool open = itemCircuits[i].gameObject.GetComponent<Valve>().open;
                var measures = itemCircuits[i].gameObject.GetComponentInChildren<Measurements>();
                float width = measures.width1;
                _circuit.Valves.Add(new ValveItem(ID, positionInCircuit, position, rotation, open, modelo, width));
            }
        }
    }

    public void LoadAllCircuitJsonFiles()
    {
        jsonFiles.Clear();
        Object[] files = Resources.LoadAll("Text/Circuit", typeof(TextAsset));
        maxNumberFile = files.Count();
        foreach (var file in files)
        {
            TextAsset fileAsset = (TextAsset)file;
            jsonFiles.Add(fileAsset);
        }
        _numberFile = files.Count() - 1;
        
    }

    internal void LoadCurrentJsonFile()
    {
        _numberFile = dropdown.dropdown.value;
        LoadJsonFile(fullpath + _numberFile + ".json");
    }

    public void ShareCircuit(Player newPlayer)
    {       
        int[] indexPlayer= new int[1];
        indexPlayer[0] = newPlayer.ActorNumber;
       
        object[] content = new object[] {tempData};
        RaiseEventOptions raiseEventOptions=new RaiseEventOptions { TargetActors = indexPlayer};        
        PhotonNetwork.RaiseEvent(ShareCircuitPlayer, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
