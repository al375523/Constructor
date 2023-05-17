using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class Valve : CircuitItem, IEditableWaterCirculation, IEditableHorizontalRotation, IEditableChangeSize
{
    public bool open=true;
    public Color waterPassClosed;
    public Color waterPassOpen;
    private Animator animator;
    public float error= 0.025f;
    // public GameObject activeValve;

    internal override void Set(Circuit circuit, float size)
    {
        allSizeGameObjects = circuit.valveGOs;
        GetAndSetAllGOWithASize(size);
        animator = currentGO.GetComponent<Animator>();
    }

    public Valve(string id, Transform initialNormal, Transform endNormal, float prevWidth, string modelo) : base(id, initialNormal, endNormal,  prevWidth, modelo)
    {
    }
    public override void ScaleItemDiameter(float currentSize, float targetSize) //TODO VER!!!
    {
      //  ChangeValve(targetSize);

        //Measurements m = activeValve.GetComponent<Measurements>();
       // m.width1 = targetSize;
       // m.width2 = targetSize;
    }
    public override void ScaleSize(float targetSize)
    {
        //ChangeValve(targetSize);
        //Measurements m = activeValve.GetComponent<Measurements>();
        //m.width1 = targetSize;
        //m.width2 = targetSize;
    }

    public void ChangeWaterPass()
    {
        open = !open;
        //Si abierto, Cambiar shader del agua       
        ColorOutlineValve();

        currentGO.GetComponent<Animator>().SetBool("waterPass", open);
    }

    public void ColorOutlineValve()
    {
        if (open)
        {
            currentGO.GetComponentInParent<Outline>().OutlineColor = waterPassOpen;
            currentGO.GetComponent<Outline>().OutlineColor = waterPassOpen;
        }
        if (!open)
        {
            currentGO.GetComponentInParent<Outline>().OutlineColor = waterPassClosed;
            currentGO.GetComponent<Outline>().OutlineColor = waterPassClosed;
        }
    }
  /*  public void GetAndSetAllValvesWithASize(float correctMeasurement)
    {
        Select(false,Color.blue);

        var allModelsWithSize= allSizeGameObjects[correctMeasurement];
        if (allModelsWithSize.Count == 0) throw new System.Exception("There isn't any valve that fit that size");


        if (activeValve != null)
        {
            activeValve.transform.parent.gameObject.SetActive(false);
            DestroyImmediate(activeValve);
        }

        gosWithCurrentSize = allModelsWithSize;
        valvulaValidaIndex = 0;


        GameObject valvulaAInstanciar = Instantiate(gosWithCurrentSize[0], this.transform);
        activeValve = valvulaAInstanciar.transform.GetChild(0).gameObject;
        activeValve.transform.parent.gameObject.SetActive(true);
        valvulaValidaIndex = 0;
        this.initialNormal = activeValve.transform.parent.gameObject.transform.Find("Init").transform;
        this.endNormal = activeValve.transform.parent.gameObject.transform.Find("End").transform;
    }

    */
    internal override void Select(bool v, Color c)
    {
        isSelected = v;
        if (currentGO)
        {
            var outline = currentGO.GetComponent<Outline>();
            if (outline != null)
            {
                ChangeOutlineColor(v, c, outline);

            }
            else
            {
                var outlines = currentGO.GetComponentsInChildren<Outline>();
                foreach (var o in outlines)
                {
                    ChangeOutlineColor(v, c, o);
                }

            }

        }
    }

    private static void ChangeOutlineColor(bool v, Color c, Outline o)
    {
        o.enabled = v;
        if (v)
            o.OutlineColor = c;
    }


    public override void SelectItemJson()
    {

        SelectItemJsonInFolder("ValvePrefabs");
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
    void IEditableWaterCirculation.EditWaterCirculation()
    {
        open = !open;
        //Si abierto, Cambiar shader del agua       
        ColorOutlineValve();
        if (animator) {
            animator.SetBool("_waterPass", open);
        }
    }

    bool IEditableWaterCirculation.CanEditWaterCirculation()
    {
        return true;
    }

    void IEditableHorizontalRotation.EditHorizontalRotation(bool shouldAdjustPosition)
    {
        gameObject.transform.Rotate(90, 0, 0);
    }

    bool IEditableHorizontalRotation.CanEditHorizontalRotation()
    {
        return nextItem == null;
    }

    void IEditableChangeSize.EditChangeSize()
    {
        NextGO();
    }

    bool IEditableChangeSize.CanEditChangeSize()
    {
        return nextItem == null;
    }
    #endregion
}