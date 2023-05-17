using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValveItem : CircuitComponents
{
    private Valve valve;
    public bool open;

    public ValveItem(string ID, int positionInCircuit, Position[] position, Rotation rotation, bool open, string modelo, float width) : base(ID,
        positionInCircuit, position, rotation, modelo, width)
    {
        this.open = open;
    }


    public override void InstantiateItem(string id, List<CircuitItem> circuitItems, Circuit circuit)
    {
        _jsonReader =  GameObject.FindObjectOfType<JsonReader>();
        //Debug.Log(id);
        GameObject valveInstantiated = Object.Instantiate(_jsonReader._circuit.InstantiatePrefabWithID(id, _jsonReader.IDToPrefabName),
            position[0].ToVector3(), Quaternion.Euler(rotation.x,rotation.y,rotation.z), GameObject.Find("Valves").transform);

        valveInstantiated.name = positionInCircuit + " " + ID;
        valve = valveInstantiated.GetComponent<Valve>();
        valve.Set(circuit, width);

        valve.ID = ID;
        valve.positionInCircuit = positionInCircuit;
        valve.pivotPosition = valveInstantiated.transform.position;
        circuitItems.Add(valve);



        if (this.open != valve.open)
        {
            valve.ChangeWaterPass();
        }
    }
}