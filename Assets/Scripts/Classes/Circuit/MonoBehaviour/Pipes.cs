using System;
using UnityEngine;

[System.Serializable]
public class Pipes : CircuitItem, IEditableLength
{
    public string pipeModelo;

    public Pipes(string id, Transform initialNormal, Transform endNormal, float prevWidth, string modelo) : base(id, initialNormal, endNormal, prevWidth, modelo)
    {
    }
    public override void ScaleItemDiameter(float currentSize, float targetSize)
    {
        Measurements measurements = GetComponentInChildren<Measurements>();

        currentSize = measurements.width1;
        Vector3 scale = transform.localScale;
        scale.y = targetSize * scale.y / currentSize;
        scale.z = targetSize * scale.z / currentSize;
        transform.localScale = scale;

        measurements.width1 = targetSize;
        measurements.width2 = targetSize;

    }


    public void ChangeLenght(GameObject go, float currentLenght, float newLength)
    {
        Vector3 scale = transform.localScale;
        scale.x = newLength * scale.x / currentLenght;
        transform.localScale = scale;
    }

    public override void SelectItemJson()
    {
        return;
    }

    #region IEditable
    bool IEditableLength.CanEditLength()
    {
        return nextItem == null;
    }

    void IEditableLength.EditLength(float delta)
    {
        var measurement = GetComponentInChildren<Measurements>();
        var go = measurement.gameObject;
        var currentLength = measurement.lenght;
        if (currentLength + delta > 0.1f)
        {
            var newLength = currentLength + delta;
            ChangeLenght(go, currentLength, newLength);
            measurement.lenght = newLength;
        }
    }
    #endregion

}

