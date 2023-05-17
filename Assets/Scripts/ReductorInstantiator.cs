using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReductorInstantiator : MonoBehaviour
{
    private string input;
    public Dropdown listaSizes;
    public ControlMorph scriptMorph;

    // Start is called before the first frame update
    void Start()
    {
        CleanDropDown();
        listaSizes.onValueChanged.AddListener(delegate { DropdownItemSelected(listaSizes); });
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
        switch (input)
        {
            case "114,3":
                //Debug.Log("60,3 - 76,1 - 88,9");
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "60,3" });
                listaSizes.options.Add(new Dropdown.OptionData() { text = "76,1" });
                listaSizes.options.Add(new Dropdown.OptionData() { text = "88,9" });
                break;
            case "133":
                //Debug.Log("57 - 60,3 - 76,1");
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "57" });
                listaSizes.options.Add(new Dropdown.OptionData() { text = "60,3" });
                listaSizes.options.Add(new Dropdown.OptionData() { text = "76,1" });
                break;
            default:
                CleanDropDown();
                break;
        }
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;

        scriptMorph.SelectFrame(input, listaSizes.options[index].text);
    }
}
