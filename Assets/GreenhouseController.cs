using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenhouseController : MonoBehaviour
{
    [HideInInspector] public GameObject greenhouse = null;
    public GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("LOADING_SECTION", GetGreenhouseObj);
        EventManager.StartListening("LOADED_FIRST_SECTION", GetGreenhouseObj);
    }

    // Update is called once per frame
    void Update()
    {
        if(greenhouse != null) greenhouse.SetActive(!loadingScreen.activeSelf);
    }

    void GetGreenhouseObj()
    {
        greenhouse = FindObjectOfType<ConstructionPanel>().gameObject;
    }   
}
