using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CircuitItem), true)]
[CanEditMultipleObjects]
public class CircuitItemEditor : Editor
{
    private Circuit circuit;
    private CircuitItem item;

    private GameObject objectToInstantiate;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        item = (CircuitItem) target;
        circuit = GameObject.Find("circuit").GetComponent<Circuit>();
        //item.isSelected = true;
        circuit.itemCircuitSelected = item;
        item.JsonReader = FindObjectOfType<JsonReader>();

        if (GUILayout.Button("Delete This"))
        {
            circuit.DeleteItem(item);
            circuit.circuitElements.Remove(item);
            //item.DestroyThis();
            //circuit.itemCircuitSelected = null;
        }

    }
}