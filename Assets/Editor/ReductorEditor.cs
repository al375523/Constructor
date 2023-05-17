using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Reductor))]
public class ReductorEditor : CircuitItemEditor
{
    private float sizeEntrada;
    private float sizeSalida;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Reductor _reductor = (Reductor)target;

        if (GUILayout.Button("Change Reductor"))
        {
            _reductor.NextGO();
        }

        if (GUILayout.Button("Flip Reductor"))
        {
           // _reductor.Flip();
        }

        sizeEntrada = EditorGUILayout.FloatField("Size Entrada", sizeEntrada);
        sizeSalida = EditorGUILayout.FloatField("Size Salida", sizeSalida);
    }
}
