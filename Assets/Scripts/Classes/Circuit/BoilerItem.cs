using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoilerItem : CircuitComponents
{
    private Boiler boiler;

    public BoilerItem(string id, int positionInCircuit, Position[] position, Rotation rotation, string modelo) :
        base(id, positionInCircuit, position, rotation, modelo, 0)
    {
    }

    public override void InstantiateItem(string id, List<CircuitItem> circuitItems, Circuit circuit)
    {
        _jsonReader = GameObject.FindObjectOfType<JsonReader>();
        GameObject boilerPrefab = _jsonReader._circuit.InstantiatePrefabWithID(id, _jsonReader.IDToPrefabName);
        //Instantiate Item
        GameObject boilerInstantiated = Object.Instantiate(boilerPrefab, position[0].ToVector3(),
            Quaternion.Euler(rotation.x, rotation.y, rotation.z), GameObject.Find("Boiler").transform);
        boilerInstantiated.name = positionInCircuit + " " + ID;
        //Getting components
        boiler = boilerInstantiated.GetComponent<Boiler>();
        if (boiler != null)
        {
            boiler.ID = ID;
            boiler.positionInCircuit = positionInCircuit;
            boiler.pivotPosition = boilerInstantiated.transform.position;
            circuitItems.Add(boiler);
            while (circuitItems.Count < 0)
            {
                circuitItems.Add(boiler);
            }
        }
    }
}