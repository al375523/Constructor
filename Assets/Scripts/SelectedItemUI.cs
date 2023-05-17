using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedItemUI : MonoBehaviour
{
    public CircuitManager circuitManager;
    public GameObject GO_UI;
    public TextMeshProUGUI idText;
    public TextMeshProUGUI widthText1; 
    public TextMeshProUGUI widthText2;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI floorText;

    void Start()
    {
        circuitManager = GameObject.FindGameObjectWithTag("Circuit").GetComponent<CircuitManager>();
    }
    void FixedUpdate()
    {
        CircuitItem item= circuitManager.circuit.itemCircuitSelected;
        if (item == null) {
            GO_UI.SetActive(false);
            return;
        }
        GO_UI.SetActive(true);
        idText.text = item.ID;
        Measurements m = item.GetComponentInChildren<Measurements>();
        widthText1.text =m.width1.ToString();
        widthText2.text =m.width2.ToString();
        heightText.text =m.lenght.ToString();
        float distance= item.DistanceToFloor();
            floorText.text = distance.ToString();

    }
}
