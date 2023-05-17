using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;
public class InstantiateControllers : MonoBehaviour
{
    public GameObject leftControllerInstance = null;
    public GameObject rightControllerInstance = null;
    public GameObject leftControllerModel;
    public GameObject rightControllerModel;
    public Transform LeftHandTrack;
    public Transform RightHandTrack;

    bool activeControllers = true;
    // Start is called before the first frame update
    void Start()
    {
        CreateControllers();
    }


    void Update()
    {
            //Si el hand tracking está activado
            if (activeControllers && OVRPlugin.GetHandTrackingEnabled())
            {
                if (leftControllerInstance != null) leftControllerInstance.SetActive(false);
                if (rightControllerInstance != null) rightControllerInstance.SetActive(false);
                activeControllers = false;
            }
            else if (!activeControllers && !OVRPlugin.GetHandTrackingEnabled())
            {
                if (leftControllerInstance != null) leftControllerInstance.SetActive(true);
                if (rightControllerInstance != null) rightControllerInstance.SetActive(true);
                activeControllers = true;
            }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (leftControllerInstance!=null && rightControllerInstance != null && leftControllerModel != null && rightControllerModel != null)
        {
            if (leftControllerInstance.activeSelf && rightControllerInstance.activeSelf)
            {
                leftControllerInstance.transform.position = LeftHandTrack.position;
                rightControllerInstance.transform.position = RightHandTrack.position;

                leftControllerInstance.transform.rotation = LeftHandTrack.rotation;
                rightControllerInstance.transform.rotation = RightHandTrack.rotation;
            }
        }
    }

    void CreateControllers()
    {
        leftControllerInstance = Instantiate(leftControllerModel, LeftHandTrack.position, LeftHandTrack.rotation);
        rightControllerInstance = Instantiate(rightControllerModel, RightHandTrack.position, RightHandTrack.rotation);
    }

    

    
}
