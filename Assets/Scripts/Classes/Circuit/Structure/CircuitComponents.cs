using System;
using System.Collections.Generic;


[Serializable]
public abstract class CircuitComponents: IComparable<CircuitComponents>
{
    public string ID;
    public int positionInCircuit;
    public Position[] position;
    public Rotation rotation;
    public string modelo;
    public float width;
  
  
    protected Circuit _circuit;
    protected JsonReader _jsonReader;
    [NonSerialized] protected Position pos;

    public CircuitComponents(string id, int positionInCircuit, Position[] position, Rotation rotation, string modelo, float width)
    {
        ID = id;
        this.positionInCircuit = positionInCircuit;
        this.position = position;
        this.rotation = rotation;
        this.modelo = modelo;
        this.width = width;
    }
    public int CompareTo(CircuitComponents obj)
    {
        if (positionInCircuit < obj.positionInCircuit) return -1;
        if (positionInCircuit > obj.positionInCircuit) return 1;
        return 0;
    }

    /// <summary>
    /// Instantiate all certain item (for example: Union).
    /// Getting its item component (for example: Union) and Measurement.
    /// Changing data from Json to item script.
    /// Change item Width and Height.
    /// </summary>
    /// <param name="id">Item to Instantiate (E.g: Union) </param>
    public abstract void InstantiateItem(string id,List<CircuitItem>circuitItems,Circuit circuit);


}
