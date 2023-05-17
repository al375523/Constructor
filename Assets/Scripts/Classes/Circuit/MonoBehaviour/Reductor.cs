using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Reductor: CircuitItem, IEditableChangeSize
{
    //public Animator myAnim;
    //public int specificFrame;
    public List<(float, float)> posibleSizes;
    public int currentEndSizeindex = 0;
    private List<(float, float)> allSizes = new List<(float, float)> { (0.032f, 0.0269f), (0.04f, 0.0269f), (0.04f, 0.0337f), (0.04f, 0.0269f), (0.04f, 0.0424f), (0.0603f, 0.0337f), (0.0603f, 0.0424f), (0.0603f, 0.0483f), (0.0761f, 0.0424f), (0.0761f, 0.0483f), (0.0761f, 0.0603f), (0.08f, 0.0483f), (0.08f, 0.0603f), (0.08f, 0.0761f), (0.1f, 0.057f), (0.1f, 0.0761f), (0.1f, 0.0889f), (0.1143f, 0.0603f), (0.1143f, 0.0761f), (0.1143f, 0.0889f), (0.125f, 0.057f), (0.125f, 0.0761f), (0.125f, 0.0889f), (0.125f, 0.108f), (0.125f, 0.0889f), (0.125f, 0.1143f), (0.15f, 0.0761f), (0.15f, 0.0889f), (0.15f, 0.108f), (0.15f, 0.133f), (0.1683f, 0.1143f), (0.1683f, 0.1397f), (0.1937f, 0.133f), (0.2191f, 0.108f), (0.2191f, 0.1143f), (0.2191f, 0.133f), (0.125f, 0.1143f), (0.1937f, 0.108f), (0.159f, 0.1143f), (0.1683f, 0.133f), (0.1397f, 0.0761f), (0.1397f, 0.108f), (0.0761f, 0.0337f), (0.15f, 0.1397f), (0.1683f, 0.0889f), (0.1f, 0.0603f), (0.125f, 0.0603f), (0.15f, 0.0603f), (0.1937f, 0.1683f) };
    public float error = 0.03f;
    public GameObject[] reductores;
    public GameObject reductorActivo;
    public float salidaJSON;
    public string reductorModelo;

    public Reductor(string id, Transform initialNormal, Transform endNormal, float prevWidth, string modelo) : base(id, initialNormal, endNormal, prevWidth, modelo)
    {
     
    }

    public override void ScaleItemDiameter(float currentSize, float targetSize)
    {
        //ChangeReductor(targetSize);
    }

    private void Awake()
    {

    }

    internal override void Set(Circuit circuit, float size)
    {
        allSizeGameObjects = circuit.reductorGOs;
        GetAndSetAllGOWithASize(size);
    }



    public override void SelectItemJson()
    {
        SelectItemJsonInFolder("ReductorPrefabs");

    }





    #region IEditable
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
