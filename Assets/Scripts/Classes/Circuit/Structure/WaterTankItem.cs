using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterTankItem : CircuitComponents
{
    private WaterTank waterTank;

    public WaterTankItem(string id, int positionInCircuit, Position[] position, Rotation rotation, string modelo) :
        base(id, positionInCircuit, position, rotation, modelo,0)
    {
    }

    public override void InstantiateItem(string id, List<CircuitItem> circuitItems,Circuit circuit)
    {
        _jsonReader = GameObject.FindObjectOfType<JsonReader>();
        GameObject waterTankPrefab = _jsonReader._circuit.InstantiatePrefabWithID(id, _jsonReader.IDToPrefabName);
        //Instantiate Item
        GameObject WaterTankInstantiated = Object.Instantiate(waterTankPrefab, position[0].ToVector3(),
            Quaternion.Euler(rotation.x, rotation.y, rotation.z), GameObject.Find("WaterTanks").transform);
        WaterTankInstantiated.name = positionInCircuit + " " + ID;
        //Getting components
        waterTank = WaterTankInstantiated.GetComponent<WaterTank>();
        if (waterTank != null)
        {
            waterTank.ID = ID;
            waterTank.positionInCircuit = positionInCircuit;
            waterTank.pivotPosition = WaterTankInstantiated.transform.position;
            circuitItems.Add(waterTank);
            while (circuitItems.Count < 0)
            {
                circuitItems.Add(waterTank);
            }
        }
    }

}