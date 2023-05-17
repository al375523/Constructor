using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReductorItem : CircuitComponents
{
    private Reductor reductor;
    public float salidaJSON;

    public ReductorItem(string id, int positionInCircuit, Position[] position, Rotation rotation, float salidaJSON, string modelo, float width) : base(id,
        positionInCircuit, position, rotation, modelo, width)
    {
        this.salidaJSON = salidaJSON;
    }

    public override void InstantiateItem(string id, List<CircuitItem>circuitItems, Circuit circuit)
    {
        _jsonReader =  GameObject.FindObjectOfType<JsonReader>();
        //Debug.Log(id);
        GameObject reductorInstantiated = Object.Instantiate(_jsonReader._circuit.InstantiatePrefabWithID(id, _jsonReader.IDToPrefabName), position[0].ToVector3(), 
            Quaternion.Euler(rotation.x,rotation.y,rotation.z), GameObject.Find("Unions").transform);
          reductorInstantiated.name = positionInCircuit + " " + ID;
        //Getting its component
        reductor = reductorInstantiated.GetComponent<Reductor>();
        reductor.Set(circuit, width);
        //Saving data
        reductor.ID = ID;
        reductor.positionInCircuit = positionInCircuit;
        reductor.pivotPosition = reductorInstantiated.transform.position;
        
        circuitItems.Add(reductor);

    }
}