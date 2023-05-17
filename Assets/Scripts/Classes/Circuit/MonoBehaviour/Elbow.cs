using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Elbow : CircuitItem, IEditableHorizontalRotation
{
    public GameObject[] codos;
    public string codoModelo;

    public Elbow(string id, Transform initialNormal, Transform pivotNormal, Transform endNormal, CircuitItem prevItem,
        CircuitItem nextItem, float initialWidth, string modelo) :
        base(id, initialNormal, pivotNormal, endNormal, prevItem, nextItem, initialWidth, modelo)
    {
    }

    internal override void Set(Circuit circuit, float size)
    {
        allSizeGameObjects = circuit.elbowGOs;
        GetAndSetAllGOWithASize(size);
        gameObject.SetActive(true);
    }
    
    public void Rotate90Degrees( bool shoulfAdjustPosition=true)
    {
        transform.Rotate(90, 0, 0);
        if(shoulfAdjustPosition)
            Relocate();
    }

    public void RotateGrades(float x)
    {
        transform.Rotate(x, 0, 0);
        Relocate();
    }

    void Relocate()
    {
        if (prevItem != null && nextItem == null)
        {
            Vector3 DNI = transform.position - initialNormal.position;
            transform.position = prevItem.endNormal.position + DNI;
        }
        else if (nextItem != null && prevItem == null)
        {
            Vector3 DNI = transform.position - endNormal.position;
            transform.position = nextItem.initialNormal.position - DNI;
        }
        else
        {
            Debug.LogWarning("Dont have references for prev or next item in circuit");
        }
    }

    public void RotateFinal90Grades()
    {
        transform.Rotate(0, 90, 0);
        Relocate();
    }

    public void RotateFinalGrades(float x)
    {
        transform.Rotate(0, x, 0);
        Relocate();
    }


    public override void ScaleItemDiameter(float currentSize, float targetSize)
    {
        GetAndSetAllGOWithASize(targetSize);
        Relocate();
        Measurements measurements = GetComponentInChildren<Measurements>();
        measurements.width1 = targetSize;
        measurements.width2 = targetSize;
    }

    internal override void Select(bool v, Color c)
    {
        isSelected = v;
        if (currentGO)
        {
            var o = currentGO.GetComponent<Outline>();
            o.enabled = v;
            if (v)
                o.OutlineColor = c;
        }
        else
        {
            foreach (var codo in codos)
            {
                codo.GetComponent<Outline>().enabled=false;
            }
        }
    }

    public override void SelectItemJson()
    {
        SelectItemJsonInFolder("ElbowPrefabs");
    }

    internal override void Highlight(bool v, Color c)
    {
        if (!isSelected) {
            var o = currentGO.GetComponent<Outline>();
            o.enabled = v;
            if (v)
                o.OutlineColor = c;
        }
    }
    #region IEditable
    void IEditableHorizontalRotation.EditHorizontalRotation(bool shouldAdjustPosition)
    {
        Rotate90Degrees(shouldAdjustPosition);
    }

    bool IEditableHorizontalRotation.CanEditHorizontalRotation()
    {
        return nextItem == null;
    }
    #endregion
}