using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeItem : CircuitComponents
{
    private Pipes pipe;
    public float height;
    protected Measurements measurement;
    public PipeItem(string id, int positionInCircuit, Position[] position, Rotation rotation, float width, float height, string modelo)
        : base(id, positionInCircuit, position, rotation, modelo, width)
    {
        this.height = height;
    }

    public override void InstantiateItem(string id, List<CircuitItem> circuitItems,Circuit circuit)
    {
        _jsonReader =  GameObject.FindObjectOfType<JsonReader>();
     
        GameObject pipePrefab = _jsonReader._circuit.InstantiatePrefabWithID(id, _jsonReader.IDToPrefabName);

        Vector3 pipePosition = (position[1] / position[0]).ToVector3();

        GameObject pipeInstantiated = Object.Instantiate(pipePrefab, pipePosition, Quaternion.Euler(rotation.x,rotation.y,rotation.z), GameObject.Find("Pipes").transform);
        pipe = pipeInstantiated.GetComponent<Pipes>();

        pipeInstantiated.name = positionInCircuit + " " + ID;
        measurement = pipeInstantiated.GetComponentInChildren<Measurements>();

        pipe.ID = ID;
        pipe.positionInCircuit = positionInCircuit;
        pipe.position = (position[1] / position[0]);
        pipe.pivotPosition = pipeInstantiated.transform.position;

        //ScalePipe(pipeInstantiated, pipePrefab.GetComponentInChildren<Measurements>().widthMeasurement, GetDistance(position[1],position[0]));
        circuitItems.Add(pipe);
        //pipe.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        if (width < height)
        {
            pipe.ScaleSize(width);
        }
        else
        {
            pipe.ScaleSize(height);
        }
        
    }

    public float GetDistance(Position pos1, Position pos2)
    {
        return Vector3.Distance(pos2.ToVector3(), pos1.ToVector3());
    }

    private static void ScalePipe(GameObject go, float currentSize, float targetSize)
    {
        Vector3 scale = go.transform.localScale;
        scale.x = targetSize * scale.x / currentSize;
        go.transform.localScale = scale;
    }
}