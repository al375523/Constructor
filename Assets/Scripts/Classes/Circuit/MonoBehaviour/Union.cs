using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// REDUCTOR CLASS
/// </summary>
[System.Serializable]
public class Union: CircuitItem
{
    
    Union union;
    
    public Union(string id, Transform initialNormal, Transform endNormal, float prevWidth, string modelo) : base(id, initialNormal, endNormal, prevWidth, modelo)
    {
     
    }

    public Animator myAnim;
    public int specificFrame;
    private string input;
    public Dropdown listaSizes;
    private List<(string, string)> allSizes;
    private List<(string, string)> searchSizes;
    public float error;

    void Start()
    {
        //CleanDropDown();
        //listaSizes.onValueChanged.AddListener(delegate { DropdownItemSelected(listaSizes); });
        specificFrame = 5;
        myAnim.Play("Change", 0, specificFrame / 35f);
        allSizes = new List<(string, string)>();
        allSizes.Add(("0,1143", "0,0603"));
        allSizes.Add(("0,1143", "0,0761"));
        allSizes.Add(("0,1143", "0,0889"));
        allSizes.Add(("0,133", "0,057"));
        allSizes.Add(("0,133", "0,0603"));
        allSizes.Add(("0,133", "0,0761"));
        error = 0.01f;
        //Debug.Log(allSizes[0]);
    }

    public void CleanDropDown()
    {
        listaSizes.options.Clear();
        listaSizes.value = -1;
    }

    public void ReadStringInput(string s)
    {
        input = s;
        Debug.Log(input);
        if (searchSizes != null)
        {
            searchSizes.Clear();
        }
        CleanDropDown();
        searchSizes = allSizes.FindAll(delegate ((string, string) s) { return Calculo(input, s); });
        foreach ((string, string) str in searchSizes)
        {
            listaSizes.options.Add(new Dropdown.OptionData() { text = str.Item2 });
        }
    }
    
    public bool Calculo(string input, (string,string) s)
    {
        return float.Parse(input) - error <= float.Parse(s.Item1) && float.Parse(s.Item1) <= float.Parse(input) + error;
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        SelectFrame(input, listaSizes.options[index].text);
    }

    public void SelectFrame(string tamañoEntrada, string tamañoSalida)
    {
        StartCoroutine(Seleccion(tamañoEntrada, tamañoSalida));
    }

    IEnumerator Seleccion(string tamañoEntrada, string tamañoSalida)
    {
        int frameCorrecto = allSizes.FindIndex(delegate ((string, string) s) { return Calculo(tamañoEntrada, s) && s.Item2 == tamañoSalida; });
        specificFrame = frameCorrecto * 5 + 5;

        for (int i = 0; i < 5; i++)
        {
            specificFrame++;
            yield return new WaitForSeconds(.1f);
            myAnim.Play("Change", 0, specificFrame / 35f);
        }
    }

    public void ChangeReductor(float sizeEntrada, float sizeSalida)
    {
        allSizes = new List<(string, string)>();
        allSizes.Add(("0,1143", "0,0603"));
        allSizes.Add(("0,1143", "0,0761"));
        allSizes.Add(("0,1143", "0,0889"));
        allSizes.Add(("0,133", "0,057"));
        allSizes.Add(("0,133", "0,0603"));
        allSizes.Add(("0,133", "0,0761"));
        //Debug.Log(medida);
        input = sizeEntrada.ToString();
        if (searchSizes != null)
        {
            searchSizes.Clear();
        }
        searchSizes = allSizes.FindAll(delegate ((string, string) s) { return Calculo(input, s); });
        foreach ((string, string) str in searchSizes)
        {
            if (str.Item2 == sizeSalida.ToString())
            {
                StartCoroutine(Seleccion(input, sizeSalida.ToString()));
            }
        }
    }
    public override void ScaleItemDiameter(float currentSize, float targetSize)
    {
        float prevWidthCalculate = targetSize;
        if (prevItem != null) prevWidthCalculate = prevItem.prevWidth;
        

        
        
        
        /*
        Vector3 scale = gameObject.transform.localScale;
        scale.y = prevWidthCalculate * scale.y / currentSize;
        scale.z = prevWidthCalculate * scale.z / currentSize;
        gameObject.transform.localScale = scale;*/
    }

    
}
