using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] panelsShow;
    [SerializeField] private GameObject canvasFull;

    //Display corresponding Canvas hiding previous
    public void ShowDisplay(int panel)
    {
        HideAllPanels();
        panelsShow[panel].SetActive(true);
    }

    //Hides all canvas
    public void HideAllPanels()
    {
        for (int i = 0; i < panelsShow.Length; i++)
        {
            panelsShow[i].SetActive(false);
        }
    }

    
}