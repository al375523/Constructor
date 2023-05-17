using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePointer : MonoBehaviour
{
    OculusInputs oculusInputs;

    // Start is called before the first frame update
    void Start()
    {
        oculusInputs = FindObjectOfType<OculusInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oculusInputs.GetSelect())
        {

        }
    }
}
