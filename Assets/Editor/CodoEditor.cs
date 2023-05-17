
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Elbow))]
public class ElbowEditor : CircuitItemEditor
{
    private float numero;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 
        Elbow _codo = (Elbow) target;
        if(GUILayout.Button("Rotate Elbow Pipe 90 grades")){
            _codo.Rotate90Degrees();
            string[] array = _codo.ID.Split(' ');
            Vector3 DNI = _codo.transform.position - _codo.initialNormal.position;
            //Posicionamos el GO2 teniendo en cuenta su distancia.
            _codo.transform.position = _codo.prevItem.endNormal.position + DNI;
        }

        if (GUILayout.Button("Change Elbow"))
        {
            _codo.GetAndSetAllGOWithASize(numero);
        }

        numero = EditorGUILayout.FloatField("Numero", numero);
    }
}