using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using System;
using Photon.Pun;
using Chiligames.MetaAvatarsPun;

public class CircuitManager : MonoBehaviour
{
    [SerializeField] private CircuitCalculator _calculator;

    [SerializeField] public UIManager _uiManager;
    public PhotonView pv;
    public GameObject handSelectorPrefab;
    private WaterPassShader waterCirculator;
    public Circuit circuit;
    public Dictionary<float,GameObject> valveGOs;
    public Dictionary<float, GameObject> elbowGOs;
    public Dictionary<float, GameObject> reductorGOs;
    OculusInputs player;
    PlayerManager playerManager;

    private void Start()
    {
        circuit.SetCircuit(GetPrefabsinFolder("ValvePrefabs"), GetPrefabsinFolder("ReductorPrefabs"), GetPrefabsinFolder("ElbowPrefabs"));
        print("start");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
        StartCoroutine(WaitUI());
        pv = GetComponent<PhotonView>();
    }

    public Dictionary<float, List<GameObject>> GetPrefabsinFolder(string folderName) {
        var GOs=Resources.LoadAll<GameObject>("Prefabs/" + folderName);
        var result = new Dictionary<float, List<GameObject>>();

        foreach (var go in GOs)
        {
            float size;
            if (go.GetComponent<Measurements>() == null)
            {
                
                if (go.GetComponentInChildren<Measurements>() != null)
                {
                    size = go.GetComponentInChildren<Measurements>().width1;
                }
                else continue;
            }
            else {
                size = go.GetComponent<Measurements>().width1;
            }
            if (!result.ContainsKey(size)) { 
                result[size] = new List<GameObject>();
            }
            result[size].Add(go);
        }
        return result;
    }

    public void ResetCircuit()
    {
        circuit.ResetCircuit();
    }

    public void DeselectItem()
    {
        circuit.DeselectItem();
    }

    public void ReubicateObject(CircuitItem previous, CircuitItem actual)
    {
        circuit.ReubicateObject(previous, actual);
    }

    public void DeleteItem(CircuitItem item)
    {
        circuit.DeleteItem(item);
    }
    public void DeleteLastItem()
    {
        circuit.DeleteLastItem();
    }

    public void SelectItem(CircuitItem item)
    {
        circuit.SelectItem(item);
    }

    internal void SelectLastItem()
    {
        circuit.SelectLastItem();
    }
    public void InstantiateItem(string objectName)
    {
        GameObject go = GetObject(objectName);
        circuit.InstantiateItem(go);
        go.SetActive(true);
        if (_uiManager != null) {
            _uiManager.ShowEdit();
            _uiManager.ShowEliminate();
        }

    }


    internal void ChangeWaterPass()
    {
        waterCirculator = GetComponent<WaterPassShader>();
        waterCirculator.ChangWaterPass();
    }

    internal void RefreshWaterPass()
    {
        waterCirculator = GetComponent<WaterPassShader>();
        var circuitElements = circuit.circuitElements;
        if (!waterCirculator.waterIsOpen)
        {
            waterCirculator.ShowClose(circuitElements.Select(x => x.gameObject).ToList());
        }
        else
        {
            List<GameObject> waterPassGOs = new List<GameObject>();
            foreach (var item in circuitElements)
            {
                Valve v = item.GetComponent<Valve>();
                if (v != null && !v.open)
                { //si es una valvula y no esta abierta paro.
                    break;
                }
                else
                {
                    waterPassGOs.Add(item.gameObject);
                }
            }
            waterCirculator.ShowOpen(waterPassGOs);
        }
    }
    public void ConfigurateCircuitComplete( CircuitItem item, string direction)
    {
         circuit.ReorderIDs();
        CircuitItem initialItem=null, endItem=null;

        if (direction== "NEXT")
        {
            initialItem = item;
            endItem = circuit.GetNextItem(item);
        }
        else // direction=previous
        {
            initialItem =circuit.GetPreviousItem(item);
            endItem = item;
        }

        if (endItem == null|| initialItem== null ) return;
        //float width= initialItem.prevItem.endNormal.get TODO conseguir el tamaño correcto
        Debug.Log(initialItem.gameObject.name + " " + endItem.gameObject.name);
        try
        {
            List<GameObject> items = _calculator.CompleteCircuit(circuit,initialItem, endItem, GetObject("Large Pipe"), GetObject("Elbow"), 0.26f,150 ); //GameObjects that complets circuit
            circuit.AddItemsToTheCircuit(items, initialItem.positionInCircuit);
        }
        catch (Exception e)
        {
            print(e);
            if (direction == "NEXT")
            {
                circuit.DeleteItem(endItem);
            }
            else // direction=previous
            {
                circuit.DeleteItem(initialItem);
            }
            ConfigurateCircuitComplete(item, direction);
        }
    }

    /// <summary>
    /// Find object in Resources/Prefabs/Circuit
    /// </summary>
    /// <param name="objectToLoad">Name Of the Object</param>
    /// <returns>Return prefab as GameObject</returns>
    public GameObject GetObject(string objectToLoad)
    {
        return Resources.Load<GameObject>("Prefabs/Circuit/" + objectToLoad);
    }
    public void SetAllPrevAndNextCircuitItems() {
        circuit.SetAllPrevAndNextCircuitItems();
    }

    public void ApplyMovementInSelectedItem()
    {
        CircuitItem item = circuit.itemCircuitSelected;
        CircuitItem next = item.nextItem;
        CircuitItem prev = item.prevItem;

        if (next != null)
        {
            float distance = Vector3.Distance(item.endNormal.position, next.initialNormal.position);
            if(distance> 0.3f)
            {
                circuit.DisconnectToItems(item, next);
                ConfigurateCircuitComplete(item, "NEXT"); //TODO VER QUE PASA SI NO HAY ESPACIO
            }
        }
        if (prev != null)
        {
            float distance = Vector3.Distance(item.initialNormal.position, prev.endNormal.position);
            if (distance > 0.3f)
            {
                circuit.DisconnectToItems(prev, item);
                ConfigurateCircuitComplete(item, "PREVIOUS"); //TODO VER QUE PASA SI NO HAY ESPACIO
            }

        }
    }

    IEnumerator WaitUI()
    {
        GameObject uiManager = null;
        while (uiManager == null)
        {
            uiManager = player.uiToShow;
            yield return null;
        }
        _uiManager = uiManager.GetComponent<UIManager>();
    }
}
