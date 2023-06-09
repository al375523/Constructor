using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PointerVR : MonoBehaviour
{
    public Transform transformOrigin;
    public float renderDistance = 20f;
    public LineRenderer lineRenderer;
    public LayerMask layerMask;
    public GameObject marker;
    public bool holding=false;

    public Material trueColor;
    public Material falseColor;

    int contador;
    Vector3 origin;
    Vector3 direction;
    Vector3 end;

    public GameObject itemBeingHighlighted;
    public GameObject lastItemHighlighted=null;
    [SerializeField] private CircuitManager circuitManager;
    [HideInInspector] public bool beingHit;
    public Color colorHighlight;  
    public Transform LeftHandTrack;
    public Transform RightHandTrack;   
    public GameObject buttonWaterPass;

    bool cooldownSelect = false;
    OculusInputs oculusInput;
    GestureDetector[] gesture;
    PhotonView pv;
    
    // Start is called before the first frame update
    void Start()
    {
        //EventManager.StartListening("BUTTON_UI", ButtonPressed);
        //EventManager.StartListening("BUTTON_UI_R", ButtonReleased);               
        //EventManager.StartListening("DESELECT_ITEM", DeselectItem);
        //gesture = GameObject.Find("GestureDetection").GetComponents<GestureDetector>();
        buttonWaterPass = GameObject.FindGameObjectWithTag("WaterButton");
        oculusInput = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        lineRenderer.enabled = false;
        holding = false;
        contador = 0;
        pv = GetComponent<PhotonView>();
        StartCoroutine(WaitUI());
    }

    // Update is called  once per frame
    void Update()
    {               
        direction = transformOrigin.forward;
        if (LeftHandTrack.GetComponent<OVRHand>().IsDataValid || RightHandTrack.GetComponent<OVRHand>().IsDataValid)
            direction = transformOrigin.right;            
        origin = transformOrigin.position;

        DoRaycast();
        if (beingHit)
        {
            if (itemBeingHighlighted != null)
            {
            itemBeingHighlighted.GetComponent<CircuitItem>().Highlight(true, colorHighlight);
                if (lastItemHighlighted == null)
                {
                    lastItemHighlighted = itemBeingHighlighted;
                }
                if (lastItemHighlighted != itemBeingHighlighted)
                {
                    lastItemHighlighted.GetComponent<CircuitItem>().Highlight(false, colorHighlight);
                    
                    lastItemHighlighted = itemBeingHighlighted;
                }
                /*if (inputManager.gestureMode && gesture[0].actualGesture.name == "Confirm")
                {
                    SelectItem();
                }*/
            }
        }
        else
        {
            if (itemBeingHighlighted != null)
            {
                itemBeingHighlighted.GetComponent<CircuitItem>().Highlight(false, colorHighlight);
                itemBeingHighlighted = null;
                lastItemHighlighted = null;
            }
        }               
    }
    void DeselectItem()
    {
        circuitManager.SelectItem(null);       
    }

    public void SelectItem()
    {             
        if (itemBeingHighlighted != null)
        {
            if (itemBeingHighlighted.GetComponent<CircuitItem>() != circuitManager.circuit.itemCircuitSelected)
            {
                pv.RPC("SelectItemPun", RpcTarget.All, null);
                /*if(circuitManager.circuit.itemCircuitSelected!=null)
                    DeselectItem();
                circuitManager.GetComponent<PhotonView>().RPC("SelectItem", RpcTarget.All, itemBeingHighlighted.GetComponent<CircuitItem>());
                GameObject.Find("EventManagerObj").GetComponent<ButtonsEvents>().ButtonEvent("Edit");*/
            }
            else
            {
                Debug.Log("Deseleccionar");
                DeselectItem();
            }
        }        
    }

    [PunRPC]
    void SelectItemPun()
    {        
        circuitManager.SelectItem(itemBeingHighlighted.GetComponent<CircuitItem>());
        GameObject.Find("EventManagerObj").GetComponent<ButtonsEvents>().ButtonEvent("Edit");
    }

    void DoRaycast()
    {
        lineRenderer.enabled = true;
        RaycastHit hit = new RaycastHit();
        lineRenderer.SetPosition(0, origin + direction * 0.04f);
        end = origin + direction * renderDistance;
        
        if(Physics.Raycast(origin, direction, out hit, renderDistance, layerMask))
        {
            lineRenderer.material = trueColor;
            end = hit.point;
            if (marker != null)
            {
                marker.SetActive(true);
                marker.GetComponentInChildren<MeshRenderer>().material = trueColor;
                marker.transform.position = hit.point;
                marker.transform.LookAt(origin);               
            }
            if (!cooldownSelect && holding && contador == 0 && !hit.collider.gameObject.tag.Equals("CircuitItem"))
            {
                if (hit.collider.transform.parent.name.Contains("Slider"))
                {
                    hit.collider.transform.parent.GetComponent<VRSlider>().hitPoint = hit.point;
                }
                contador = 1;
                if(hit.collider.transform.parent.GetComponent<ButtonListener>()!=null)
                    hit.collider.transform.parent.SendMessage("OnRaycastReceived");
                StartCoroutine(CooldownSelect());
            }
            
            //Highlight Item if it has the tag "CircuitItem"
            if (hit.collider.gameObject.CompareTag("CircuitItem"))
            {
                beingHit = true;             
                itemBeingHighlighted = hit.collider.gameObject.transform.parent.gameObject;
            }
            else
            {
                itemBeingHighlighted = null;
            }
        }
        else 
        {
            beingHit = false;
            marker.SetActive(false);
            lineRenderer.material = falseColor;
        }                
        lineRenderer.SetPosition(1, end);
    }

    IEnumerator CooldownSelect()
    {
        cooldownSelect = true;
        yield return new WaitForSeconds(0.5f);
        cooldownSelect = false;
    }

    public void ButtonPressed()
    {     
        holding = true;
    }

    public void ButtonReleased()
    {      
        holding = false;
         contador = 0;

    }

    IEnumerator WaitUI()
    {
        GameObject uiManager = null;
        while (uiManager == null)
        {
            uiManager = oculusInput.uiToShow;
            yield return null;
        }     
    }
}