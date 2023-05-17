using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CircuitItem : MonoBehaviour, IComparable<CircuitItem>
{
    #region Variables

    public string ID;
    public int positionInCircuit;
    public Position position;
    public Rotation rotation;
    protected float width, height;
    public Transform initialNormal;
    public Transform endNormal;
    public CircuitItem prevItem;
    public CircuitItem nextItem;
    private JsonReader _jsonReader;
    public Vector3 pivotPosition;
    protected Measurements measurement;
    protected bool isSelected;
    public float prevWidth;
    public string currentModel;
    internal Dictionary<float, List<GameObject>> allSizeGameObjects;
    internal List<GameObject> gosWithCurrentSize = new List<GameObject>();
    internal int indexCurrentGO=-1;
    public GameObject currentGO;
    List<Outline> outlines;
    public JsonReader JsonReader
    {
        get => _jsonReader;
        set => _jsonReader = value;
    }

    #endregion


    #region Constructors

    public CircuitItem()
    {
    }

    public CircuitItem(string id, Transform initialNormal, Transform endNormal, float prevWidth, string modelo)
    {
        ID = id;
        this.initialNormal = initialNormal;
        this.endNormal = endNormal;
        this.prevWidth = prevWidth;
        this.currentModel = modelo;
    }

    public CircuitItem(string id, Transform initialNormal, Transform pivotNormal, Transform endNormal,
        CircuitItem prevItem, CircuitItem nextItem, float prevWidth, string modelo)
    {
        ID = id;
        this.initialNormal = initialNormal;
        this.endNormal = endNormal;
        this.prevItem = prevItem;
        this.nextItem = nextItem;
        this.prevWidth = prevWidth;
        this.currentModel = modelo;
    }

    public CircuitItem(string id, Transform initialNormal, Transform pivotNormal, Transform endNormal,
        CircuitItem prevItem, CircuitItem nextItem, Position position, Rotation rotation, string modelo)
    {
        ID = id;
        this.initialNormal = initialNormal;
        this.endNormal = endNormal;
        this.prevItem = prevItem;
        this.nextItem = nextItem;
        this.position = position;
        this.rotation = rotation;
        this.currentModel = modelo;
    }

    #endregion



    public override string ToString()
    {
        return ID + " : located at " + "starts at: " + initialNormal.position + " and ends at: " + endNormal.position;
    }


    public virtual void InstantiateItem(string idData, Position positionToInstantiate, Rotation itemRotation)
    {
        _jsonReader = FindObjectOfType<JsonReader>();
        if (_jsonReader == null) Debug.LogWarning("Json Reader is null");
        GameObject prefab = InstantiatePrefabWithID(idData, _jsonReader.IDToPrefabName);
        GameObject itemInstantiated =
            GameObject.Instantiate(prefab, positionToInstantiate.ToVector3(), itemRotation.ToQuaternion());
        // AddComponentToInstantiate(idData, itemInstantiated);
        _jsonReader.SearchAndAssignParent(idData, itemInstantiated);
    }

    public GameObject InstantiatePrefabWithID(string id, Dictionary<string, string> IDToPrefabName)
    {
        if (!IDToPrefabName.ContainsKey(id)) Debug.LogWarning("There is no prefab with that ID");

        return LoadPrefab("Circuit/", IDToPrefabName[id]);
    }

    public GameObject LoadPrefab(string folder, string prefSearch)
    {
        return Resources.Load<GameObject>("Prefabs/" + folder + prefSearch);
    }


    public void SetOutline()
    {
        if (!isSelected) GetComponent<Outline>().enabled = false;
        
        if (isSelected)
        {
            GetComponent<Outline>().enabled = true;
            GetComponent<Outline>().OutlineColor = Color.blue;
            GetComponent<Outline>().OutlineWidth = 10f;
        }
        
    }


    /// <summary>
    /// Move pipe to position after scaling
    /// </summary>
    /// <param name="go">Pipe to move</param>
    public void MoveToInitialPosition(Transform tEndPosPrevItem)
    {
        /*var axis = endNormal.right;
        var startPos = tEndPosPrevItem.position;
        var finalPos = tEndPosPrevItem.position + tEndPosPrevItem.right;
        Vector3 lookPos = finalPos - startPos;
        Quaternion lookRot = Quaternion.FromToRotation(axis, lookPos);
        transform.rotation = lookRot;*/
 

        Quaternion rotation = Quaternion.LookRotation(tEndPosPrevItem.forward, tEndPosPrevItem.up);
        transform.rotation = rotation;
        //transform.Rot
        
        Vector3 offset = currentGO.transform.position - initialNormal.position;
        currentGO.transform.position = tEndPosPrevItem.position + offset;
    }

    /// <summary>
    /// Change length of pipe
    /// </summary>
    /// <param name="length">Set End Position at the end of tthe pipe</param>

    public float DistanceToFloor()
    {
        Measurements m = GetComponentInChildren<Measurements>();
        float height = m.height_meters / 2;

        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Floor"); ;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            float d = Vector3.Distance(this.transform.position, hit.point);
            if (this.transform.position.y - height > hit.point.y)
            {
                return d - height;
            }
            else
            {
                return -1;
            }
        }
        return Mathf.Infinity;
    }

    public void SelectItemHands()
    {
        isSelected = true;
    }
    public virtual void ScaleItemDiameter(float currentSize, float targetSize)

    {
        throw new NotImplementedException();
    }

    public virtual void ScaleSize(float targetSize)
    {
        throw new NotImplementedException();
    }

    internal virtual void Select(bool v,Color c)
    {
        isSelected = v;
        var o = GetComponent<Outline>();
        if (o != null) {
            o.enabled = v;
            if (v)
                o.OutlineColor = c;
        }
        var os = GetComponentsInChildren<Outline>();
        if (os != null)
        {
            foreach (var item in os)
            {
                item.enabled = v;
                if (v)
                    item.OutlineColor = c;
            }

        }
    }

    internal virtual void Highlight(bool v,Color c)
    {
        if (!isSelected)
        {
            var o = GetComponent<Outline>();
            o.enabled = v;
            if (v)
                o.OutlineColor = c;
        }
    }

    public virtual void SelectItemJson()
    {
        throw new NotImplementedException();
    }

    internal virtual void Set(Circuit circuit, float size)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        print("on trigger enter" + other.gameObject.name);
    }

    public void GetAndSetAllGOWithASize(float correctMeasurement)
    {
        Select(false, Color.blue);

        var allModelsWithSize = allSizeGameObjects[correctMeasurement];
        if (allModelsWithSize.Count == 0) throw new System.Exception("There isn't any valve that fit that size");


        if (currentGO != null)
        {
            currentGO.transform.parent.gameObject.SetActive(false);
            DestroyImmediate(currentGO);
        }

        gosWithCurrentSize = allModelsWithSize;


        currentGO = Instantiate(gosWithCurrentSize[0], this.transform);
        currentGO.SetActive(true);
        currentModel = currentGO.name;
        indexCurrentGO = 0;

        SetInitAndEndPos();
    }

    private void SetInitAndEndPos()
    {
        if (currentGO.transform.Find("Init") == null)
        {
            initialNormal = currentGO.transform.GetChild(0).Find("Init").transform;
        }
        else initialNormal = currentGO.transform.Find("Init").transform;

        if (currentGO.transform.Find("End") == null)
        {
            endNormal = currentGO.transform.GetChild(0).Find("End").transform;
        }
        else endNormal = currentGO.transform.Find("End").transform;
    }

    public void NextGO()
    {
        indexCurrentGO++;
        if (indexCurrentGO >= gosWithCurrentSize.Count) indexCurrentGO = 0;
        if (currentGO != null) {
            DestroyImmediate(currentGO);
        }
        currentGO = Instantiate(gosWithCurrentSize[indexCurrentGO], this.transform);
        currentGO.SetActive(true);
        currentModel = currentGO.name;
        SetInitAndEndPos();

        MoveToInitialPosition(prevItem.endNormal);

    }

    internal void SelectItemJsonInFolder(string v)
    {
        if (currentGO != null)
        {
            DestroyImmediate(currentGO);
        }
        currentModel = currentModel.Replace("(Clone)", "");
        Debug.Log(v + "/" +currentModel);
        var go = LoadPrefab(v+"/", currentModel);
        print(currentModel);
        currentGO = Instantiate(go, this.transform);
        currentGO.SetActive(true);
        SetInitAndEndPos();
    }


    public int CompareTo(CircuitItem obj)
    {
        if (positionInCircuit < obj.positionInCircuit) return -1;
        if (positionInCircuit > obj.positionInCircuit) return 1;
        return 0;
    }
}
