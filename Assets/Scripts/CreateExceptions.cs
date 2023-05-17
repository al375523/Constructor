using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateExceptions : MonoBehaviour
{
    public GameObject reductorButton;
    public GameObject elbowButton;

    List<CircuitItem> circuitElements = new List<CircuitItem>();
    // Start is called before the first frame update
    void Start()
    {
        circuitElements = GameObject.FindGameObjectWithTag("Circuit").GetComponent<Circuit>().circuitElements;
    }

    // Update is called once per frame
    void Update()
    {
        if (circuitElements.Count > 0)
        {
            reductorButton.SetActive(true);
            elbowButton.SetActive(true);
        }

        else
        {
            reductorButton.SetActive(false);
            elbowButton.SetActive(false);
        }
    }
}
