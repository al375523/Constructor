using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElbowItem : CircuitComponents
{
    private Elbow elbow;

    public ElbowItem(string id, int positionInCircuit, Position[] position, Rotation rotation, string modelo, float width) : base(id,
        positionInCircuit, position, rotation, modelo, width)
    {
    }

    public override void InstantiateItem(string id, List<CircuitItem>circuitItems, Circuit circuit)
    {
        _jsonReader =  GameObject.FindObjectOfType<JsonReader>();
        //Debug.Log(id);
        GameObject elbowPref = _jsonReader._circuit.InstantiatePrefabWithID(id,_jsonReader.IDToPrefabName); //prefab
        GameObject elbowInstantiated = Object.Instantiate(elbowPref, position[0].ToVector3(), 
            Quaternion.Euler(rotation.x,rotation.y,rotation.z), GameObject.Find("Elbows").transform); //Instantiate
       
        elbowInstantiated.name = positionInCircuit + " " + ID; //Setting name of GameObject In Scene
        
        elbow = elbowInstantiated.GetComponent<Elbow>(); // Get component
        elbow.Set(circuit, width);
        //Assign values
        elbow.ID = ID;
        elbow.positionInCircuit = positionInCircuit;
        elbow.pivotPosition = elbowInstantiated.transform.position;
        circuitItems.Add(elbow);
        //elbow.prevWidth = circuitItems[circuitItems.Count - 2].GetComponentInChildren<Measurements>().width2;


    }
}