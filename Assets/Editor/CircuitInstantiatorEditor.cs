using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CircuitManager))]
public class CircuitInstantiatorEditor : Editor
{
    private CircuitManager circuitManager;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        #region Initialization
        circuitManager = (CircuitManager) target;
        circuitManager.circuit.parent = circuitManager.gameObject;
        #endregion
        #region Instantiate Items
        GUILayout.BeginVertical();
        GUILayout.Label("\n"+"Construction Options");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Pump", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.InstantiateItem("Pump");
        }
        if (GUILayout.Button("Elbow", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.InstantiateItem("Elbow");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Large Pipe", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.InstantiateItem("Large Pipe");
        }
        if (GUILayout.Button("Small Pipe", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.InstantiateItem("Small Pipe");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reductor", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.InstantiateItem("Reductor");
        }
        if (GUILayout.Button("Valve", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.InstantiateItem("Valve");
        }
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Complete Circuit", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.ConfigurateCircuitComplete(circuitManager.circuit.itemCircuitSelected,"NEXT");
        }

        if (GUILayout.Button("Apply Circuit Movement", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.ApplyMovementInSelectedItem();
        }
        GUILayout.EndVertical();
        #endregion
        #region Circuit Delete
        GUILayout.BeginVertical();
        GUILayout.Label("\nDelete Section");
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Circuit", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.ResetCircuit();
        }
        //Delete last item from list
        if (GUILayout.Button("Delete Last Item", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.DeleteLastItem();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        #endregion

        GUILayout.Label("");
        if (GUILayout.Button("Deselect Item", GUILayout.Width(175), GUILayout.Height(25)))
        {
            circuitManager.DeselectItem();
        }
    }
}