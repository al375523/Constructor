using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Valve))]
public class ValveEditor : CircuitItemEditor
{
    private float medida;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IEditableWaterCirculation water = (IEditableWaterCirculation)target;
        Valve _valve = (Valve) target;
        if (GUILayout.Button("Open or close water"))
        {
            water.EditWaterCirculation();
        }

        if (GUILayout.Button("Change Model"))
        {
            _valve.NextGO();
        }

        if (GUILayout.Button("Change Valve"))
        {
            _valve.GetAndSetAllGOWithASize(medida);
        }

        medida = EditorGUILayout.FloatField("Medidas", medida);
    }
}