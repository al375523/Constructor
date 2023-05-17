using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircuitCalculator))]
public class CircuitCalculatorEditor : Editor
{
    private CircuitCalculator c;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        c = (CircuitCalculator)target;


        if (GUILayout.Button("CircuitComplete"))
        {
            c.CompleteCircuit();
        }

    }
}
