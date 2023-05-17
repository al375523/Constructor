using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownArrow : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Toggle toggle;
    public int arrowDirection;
    public JsonReader jsonReader;


    private void Awake()
    {
         FindObjectOfType<JsonReader>().Set(this);
    }
    public void ChangeValueOfDropdown() {
        print("cambio!");
        int newValue = dropdown.value+ arrowDirection;
        if (newValue == dropdown.options.Count) {
            newValue = 0;
        }
        if (newValue < 0) {
            newValue = dropdown.options.Count - 1;
        }
        dropdown.value = newValue;

    }
    public void ChangeSaveOption() {
        toggle.isOn = !toggle.isOn;
    }

    internal void Set(IEnumerable<string> enumerable)
    {
        dropdown.ClearOptions();
        foreach (var item in enumerable)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }
    }

    internal void AddNewOption(string s) {
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = s });

    }
}
