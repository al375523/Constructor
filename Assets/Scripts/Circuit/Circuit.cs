using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using Debug = UnityEngine.Debug;


[System.Serializable]
public class Circuit : MonoBehaviour
{
    [Header("Circuit Positions")] public Transform initialCircuitPosition;
    [Header("Instantiate")] public List<CircuitItem> circuitElements = new List<CircuitItem>();
    public CircuitItem itemCircuitSelected = null;

    public int instantiatedID = 0;
    public Transform endCircuitPosition;
    public GameObject parent;

    public Dictionary<float, List<GameObject>> valveGOs;
    public Dictionary<float, List<GameObject>> elbowGOs;
    public Dictionary<float, List<GameObject>> reductorGOs;


    private void Awake()
    {
        parent = gameObject;
        itemCircuitSelected = null;
    }


    #region List and Parenting Methods

    /// <summary>
    /// Will insert this item at the list and set this GO as parent
    /// </summary>
    /// <param name="index">Index to insert in list and ID's</param>
    /// <param name="item">Item to add to list</param>
    public void InsertToListAndParent(int index, CircuitItem item)
    {
        if (index >= circuitElements.Count)
        {
            circuitElements.Add(null);
            circuitElements[circuitElements.Count - 1] = item;
        }
        else
        {
            circuitElements.Insert(index, item);
        }      
        item.transform.parent = parent.transform;
    }

    internal void SetCircuit(Dictionary<float, List<GameObject>> valves, Dictionary<float, List<GameObject>> reductors, Dictionary<float, List<GameObject>> elbows)
    {
        valveGOs = valves;
        elbowGOs = elbows;
        reductorGOs = reductors;

    }

    #endregion

    #region Item Attributes

    /// <summary>
    /// Will calculate the distance and rotations from both game objects to 
    /// </summary>
    /// <param name="previous">Previous item for getting its end</param>
    /// <param name="actual">Newest item</param>
    public void ReubicateObject(CircuitItem previous, CircuitItem actual)
    {
        //Variables
        Vector3 prevEndNormal = previous.endNormal.forward;
        Vector3 actualInitialNormal = actual.initialNormal.forward;
        Transform actualTransform = actual.transform;

        Quaternion rotation = Quaternion.LookRotation(previous.endNormal.forward, previous.endNormal.up);
        actual.transform.rotation = rotation;

        //Calculamos la distancia del GameObject 2.
        Vector3 DNI = actualTransform.position - actual.initialNormal.position;

        //Posicionamos el GO2 teniendo en cuenta su distancia.
        actualTransform.position = previous.endNormal.position + DNI;
    }

    /// <summary>
    /// Method for calculate if InitialNormal from an item is near from the EndNormal of the other object 
    /// </summary>
    /// <param name="item2">The item from will need the initial normal </param>
    /// <param name="item1">The item from will need the end normal </param>
    /// <param name="offset">Offset of the maximum distance we want </param>
    /// <returns>true if is is nearer than the offset, false if it is further</returns>
    public bool CalculateIfNear(CircuitItem item2, CircuitItem item1, float offset)
    {
        bool isNear = false; //Setting as default

        float distance =
            Vector3.Distance(item2.initialNormal.position,
                item1.endNormal.position); //Distance (Magnitude Vector) between normals
        if (distance < offset) //If distance is smaller than offset
        {
            isNear = true;
        }

        return isNear;
    }

    /// <summary>
    /// Will Set on each new item new and next item
    /// </summary>
    /// <param name="prevItem">Previous item in circuit</param>
    public void SetPrevAndNextItem(CircuitItem curItem , CircuitItem prevItem)
    {
        curItem.prevItem = prevItem;
        prevItem.nextItem = curItem;
       // circuitElements[_instantiatedObject.GetComponent<CircuitItem>().positionInCircuit].nextItem = _instantiatedObject.GetComponent<CircuitItem>();
 }

    public void SetAllPrevAndNextCircuitItems()
    {
        if (circuitElements.Count < 2) return;
        for (int i = 1; i < circuitElements.Count; i++)
        {
            if (i == circuitElements.Count - 1)
            {
                circuitElements[i].prevItem = circuitElements[i - 1];
                return;
            }

            circuitElements[i - 1].nextItem = circuitElements[i];
            circuitElements[i].prevItem = circuitElements[i - 1];
            circuitElements[i].nextItem = circuitElements[i + 1];
        }
    }

    #endregion
    internal void ApplyMovement(CircuitItem itemCircuitSelected)
    {
        
    }

    /// <summary>
    /// Will instantiate the Object in Scene
    /// </summary>
    /// <param name="objectName">Name of the GameObject in Resources/Prefabs/Circuit, will be used to set its ID as well</param>
    public void InstantiateItem(GameObject obj)
    {
        CircuitItem curItem;
        if (circuitElements.Count > 0) // >=2nD item
        {
            curItem=InstantiateCircuitItem(obj);
           if (curItem.positionInCircuit + 1 < circuitElements.Count &&
                    CalculateIfNear(circuitElements[curItem.positionInCircuit + 1], curItem, 0.02f))
                {
                    circuitElements[curItem.positionInCircuit + 1].prevItem = curItem;                 
                }                     
        }
        else
        {
            curItem=InstanciateFirst(obj);
            
        }
        SelectItem(curItem);
        AssignNameParentAndSelectedItem(curItem);
    }

    private void AssignNameParentAndSelectedItem(CircuitItem curItem)
    {
        curItem.transform.gameObject.name = curItem.positionInCircuit + " " + curItem.ID;
        curItem.transform.parent = parent.transform;
        instantiatedID = curItem.positionInCircuit;
    }

    internal void SelectItem(CircuitItem item)
    {
        itemCircuitSelected = item;

        foreach (var elem in circuitElements)
        {
            if (item == elem) {
                elem.Select(true,Color.blue);
            }
            else
            {
                elem.Select(false,Color.blue);
            }
        }
    }

    internal void SelectLastItem()
    {
        SelectItem(circuitElements[circuitElements.Count - 1]);
    }

    private CircuitItem InstanciateFirst(GameObject prefab)
    {        
        var instanciateGO = InstantiateGO(prefab, Vector3.zero, prefab.transform.rotation,150, this);
        var curItem = instanciateGO.GetComponent<CircuitItem>();
        //lastItem.ID = objectName;
        curItem.positionInCircuit = 0;
        circuitElements.Add(curItem);
        return curItem;
    }

    #region Delete items, Reset circuit and Instanciate circuit
    /// <summary>
    /// Set item selected (bool) as false and itemCircuitSelected (CircuitItem) as null 
    /// </summary>
    public void DeselectItem()
    {//TODO VER
        itemCircuitSelected = null;
    }

    /// <summary>
    /// Saves info of the deleted Object
    /// </summary>
    public void ResetCircuit()
    {
        DeselectItem();
        foreach (CircuitItem item in gameObject.GetComponentsInChildren<CircuitItem>())
        {
            DeleteItem(item);
        }

        circuitElements.Clear();
    }

    /// <summary>
    /// Delete last item from the list and scene
    /// </summary>
    public void DeleteLastItem()
    {
        if (circuitElements.Count > 0) {
            var item=circuitElements[circuitElements.Count - 1];
            DeleteItem(item.GetComponent<CircuitItem>());
        }
    }

    /// <summary>
    /// Delete Selected item and reorder all IDs
    /// </summary>
    /// <param name="item"></param>
    public void DeleteItem(CircuitItem item)
    {
        try
        {      
            DestroyImmediate(item.gameObject);
            circuitElements.Remove(item);
            if (circuitElements.Count > 0)
            {
                SelectItem(circuitElements[circuitElements.Count - 1]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    /// <summary>
    /// Instantiate at least 2nd Item
    /// </summary>
    /// <param name="objectName">Name of Item to set ID</param>
    CircuitItem InstantiateCircuitItem(GameObject obj)
    {
        if (itemCircuitSelected == null) throw new Exception("It must have an previous item");
        CircuitItem prevItem = itemCircuitSelected;
        CircuitItem curItem = InstantiateGO(obj, prevItem.transform.position, obj.transform.rotation, prevItem.gameObject.GetComponentInChildren<Measurements>().width2, this);

        var position = prevItem.positionInCircuit;
        position += 1;
        curItem.positionInCircuit = position; //Setting its ID
        SetPrevAndNextItem(curItem, prevItem);
        InsertToListAndParent(curItem.positionInCircuit, curItem); //Insert to main List and parent 
        ReubicateObject(prevItem, curItem); //Reubicate through normals 
        ReorderIDs();
        curItem.ScaleItemDiameter(curItem.prevWidth, curItem.prevItem.gameObject.GetComponentInChildren<Measurements>().width2);
        curItem.gameObject.SetActive(true);
        return curItem;
    }

    public static CircuitItem InstantiateGO(GameObject obj, Vector3 pos, Quaternion rot, float width, Circuit c)
    {
        CircuitItem curItem = Instantiate(obj, pos, rot).GetComponent<CircuitItem>(); //Instantiate Prefab
        curItem.Set(c, width);

        return curItem;
    }
    internal void ReorderIDs()
    {
        for (int i = 0; i < circuitElements.Count; i++)
        {
            if (circuitElements[i].positionInCircuit != i) circuitElements[i].positionInCircuit = i;
            circuitElements[i].transform.gameObject.name = circuitElements[i].positionInCircuit + " " + circuitElements[i].ID;
        }
        circuitElements.Sort();
    }
    #endregion



    /// <summary>
    /// Get other list that completes the empty space and adds them to circuit list
    /// </summary>
    /// <param name="pipeType">Name of Pipe to instantiate</param>
    public void AddItemsToTheCircuit(List<GameObject> items, int initialPosition)
    {
        List<CircuitItem> itemsCircuitForComplete = new List<CircuitItem>(); //For getting its component
        foreach (GameObject go in items)
        {
            itemsCircuitForComplete.Add(go.GetComponent<CircuitItem>()); //Geting each component
        }

        for (int i = 0; i <itemsCircuitForComplete.Count; i++)
        {
            int posToAdd = i + 1 + initialPosition;
            var itemToAdd = itemsCircuitForComplete[i];
            circuitElements.Insert(posToAdd, itemToAdd); //Adding to main list of items
            AssignNameParentAndSelectedItem(itemToAdd);
        }
        ReorderIDs(); //Reordering  IDs for adding prev and next items 

        //Connect elements
        for (int i = initialPosition; i <= initialPosition + itemsCircuitForComplete.Count; i++)
        {
            ConnectToItems(circuitElements[i], circuitElements[i + 1]);
        }
    }
    #region Get Next and Previous Item
    /// <summary>
    /// Get the next item from the selected item. Even if it is not connected
    /// </summary>
    internal CircuitItem GetNextSelectedItem()
    {
        return GetNextItem(itemCircuitSelected);
    }

    /// <summary>
    /// Get the previous item from the selected item. Even if it is not connected
    /// </summary>
    internal CircuitItem GetPreviousSelectedItem()
    {
        return GetPreviousItem(itemCircuitSelected);
    }

    /// <summary>
    /// Get the next item even if it is not connected
    /// </summary>
    internal CircuitItem GetNextItem(CircuitItem item)
    {
        int position = circuitElements.IndexOf(item);
        if (circuitElements.Count != position + 1)
            return circuitElements[position + 1];
        return null;
    }

    /// <summary>
    /// Get the previous item, Even if it is not connected
    /// </summary>
    internal CircuitItem GetPreviousItem(CircuitItem item)
    {
        int position = circuitElements.IndexOf(item);
        if (circuitElements.Count != 0)
            return circuitElements[position - 1];
        return null;
    }

    #endregion  
    internal void DisconnectToItems(CircuitItem previous, CircuitItem current)
    {
        previous.nextItem = null;
        current.prevItem = null;
    }

    internal void ConnectToItems(CircuitItem previous, CircuitItem current)
    {
        previous.nextItem = current;
        current.prevItem = previous;
    }

    public void ChangeModels(List<CircuitComponents> _componentsArrays)
    {
        for (int i = 0; i < circuitElements.Count; i++)
        {
            for (int j = 0; j < _componentsArrays.Count; j++)
            {
                if (_componentsArrays[j].positionInCircuit == circuitElements[i].positionInCircuit)
                {
                    circuitElements[i].currentModel = _componentsArrays[j].modelo;
                    circuitElements[i].SelectItemJson();
                }
            }
        }
    }
   
}