using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ValvulaInstantiator : MonoBehaviour
{

    public GameObject[] valvulas;
    private GameObject valvulaActiva;

    private string input;
    public Dropdown listaSizes;

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
        listaSizes.options.Add(new Dropdown.OptionData() { text = " " });
    }

    public void ReadStringInput(string s)
    {
        input = s;
        Debug.Log(input);
        switch (input)
        {
            case "723,9":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN32_01" });
                break;
            case "833,12":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN32_02" });
                break;
            case "944,88":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN40_01" });
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN40_02" });
                break;
            case "998,22":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN40_03" });
                break;
            case "1094,74":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN40_04" });
                break;
            case "1300,48":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN50_01" });
                break;
            case "1384,3":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN50_02" });
                break;
            case "2095,5":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN80" });
                break;
            case "2560,32":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN100_01" });
                break;
            case "2720,34":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN100_02" });
                break;
            case "3175":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN125_01" });
                break;
            case "3345,18":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN125_02" });
                break;
            case "3810":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN150_01" });
                break;
            case "4046,22":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN150_02" });
                break;
            case "4645,66":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN56" });
                break;
            case "6614,16":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN250" });
                break;
            case "7866,38":
                CleanDropDown();
                listaSizes.options.Add(new Dropdown.OptionData() { text = "DN300" });
                break;
            default:
                CleanDropDown();
                break;
        }
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        string modelo = listaSizes.options[index].text;
        if (modelo == " ")
        {
            return;
        }

        GameObject valvulaCorrecta = valvulas.Where(x => x.name == modelo).SingleOrDefault();

        //INSTANTIATE
        if (valvulaActiva != null)
        {
            Destroy(valvulaActiva);
        }
        valvulaActiva = Instantiate(valvulaCorrecta, new Vector3(0, 0.2f, -0.2f), Quaternion.identity);
    }
}
