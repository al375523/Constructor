using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject editBtn;
    public GameObject eliminateBtn;
    public GameObject completeCircuit;
    public void ShowEdit() => editBtn.SetActive(true);
    public void ShowEliminate() => eliminateBtn.SetActive(true);
    public void ShowComplete() => completeCircuit.SetActive(true);
    
    public void HideEdit() => editBtn.SetActive(false);
    public void HideEliminate() => eliminateBtn.SetActive(false);
    public void HideComplete() => completeCircuit.SetActive(false);


}
