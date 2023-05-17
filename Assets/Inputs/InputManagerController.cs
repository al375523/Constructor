using System;
using System.Collections;
using UnityEditor;
using UnityEngine;


public class InputManagerController : MonoBehaviour
{
    public ControlManager controls;
    public GameObject menu;
    
    GameObject mainCamera;
    GameObject cameraParent;
    Vector2 move;
    Vector2 rotation;

    public GameObject character;
    public GameObject uiToShow;
    public bool isEnabled;
    public CircuitManager circuitManager;
    public PointerVR pointerVR;
    public Transform LeftHandTrack;
    public Transform RightHandTrack;
    public bool gestureMode = false;
    public float uiDistance = 1.5f;
    
    GestureDetector[] gestures;
    GameObject cameraVR;
    GameObject player;
    GameObject canvasPos;

    LineRenderer line;
    bool confirmCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        controls = new ControlManager();
        controls.Gameplay.Enable();
        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Rotation.performed += ctx => rotation = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotation.canceled += ctx => rotation = Vector2.zero;
        controls.Gameplay.Rotation.performed += ctx => Rotate();
        controls.Gameplay.ButtonPressed.performed += ctx => EventManager.TriggerEvent("BUTTON_UI");
        controls.Gameplay.ButtonPressed.canceled += ctx => EventManager.TriggerEvent("BUTTON_UI_R");
        controls.Gameplay.ShowUI.performed += ctx => ActivateUI();
        controls.Gameplay.SelectItem.performed += ctx => EventManager.TriggerEvent("SELECT_ITEM");
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //controls.Gameplay.SelectItem.performed += ctx => SelectItem(pointerVR.itemBeingHighlighted.GetComponent<CircuitItem>());
        mainCamera = Camera.main.gameObject;
        cameraParent = mainCamera.transform.parent.parent.gameObject;
        //gestures = GameObject.Find("GestureDetection").GetComponents<GestureDetector>();
        player = GameObject.FindGameObjectWithTag("Player");
        uiToShow.GetComponentInChildren<SelectedItemUI>().gameObject.SetActive(false);
        line = pointerVR.gameObject.GetComponent<LineRenderer>();
        confirmCooldown = false;

        canvasPos = GameObject.FindGameObjectWithTag("Ubication");
    }

    // Update is called once per frame
    void Update()
    {
        if(LeftHandTrack.GetComponent<OVRHand>().IsDataValid || RightHandTrack.GetComponent<OVRHand>().IsDataValid)
        {
            gestureMode = true;
        }
        else
        {
            gestureMode = false;          
        }
        if (!menu.activeSelf)
        {
            Move();
            /*
            if (gestureMode && gestures[0].actualGesture.name == "Pointing")
            {
                Vector3 movement = mainCamera.transform.forward * Time.deltaTime * 1.0f;
                player.transform.position += movement;
            }*/
        }

        /*
        if(gestureMode && gestures[0].actualGesture.name=="Confirm" && line.enabled && !confirmCooldown)
        {
            confirmCooldown = true;
            EventManager.TriggerEvent("BUTTON_UI");
            StartCoroutine(ConfirmCooldown());
        }*/
    }

    IEnumerator ConfirmCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        EventManager.TriggerEvent("BUTTON_UI_R");
        yield return new WaitForSeconds(1f);
        confirmCooldown = false;
    }

    private void Move()
    {
        float x = move.x;
        float y = move.y;

        Vector3 vectorForward = mainCamera.transform.forward;
        Vector3 vectorRight = mainCamera.transform.right;
        Vector3 movement = ((vectorRight * x + vectorForward * y).normalized) * Time.deltaTime;
        cameraParent.transform.position += movement;
    }

    void Rotate()
    {
        Vector2 r = new Vector2(0, 0);
        if (rotation.x >= 0.1f)
        {
            r = new Vector2(0, 45);
        }
        else if (rotation.x <= -0.1f)
        {
            r = new Vector2(0, -45);
        }

        cameraParent.transform.Rotate(r, Space.World);
    }

    void ActivateUI()
    {
        isEnabled = !isEnabled;
        uiToShow.transform.position = new Vector3(canvasPos.transform.position.x, canvasPos.transform.position.y, canvasPos.transform.position.z);
        if (isEnabled) 
        { 
            uiToShow.transform.LookAt(character.transform);
        }
        uiToShow.SetActive(isEnabled);
    }

    /// <summary>
    /// Item selection pressing button  "X" from oculus controller
    /// </summary>
    /// <param name="circuitItem">Item being highlighted</param>
    void SelectItem(CircuitItem circuitItem)
    {
        /* Debug.Log("Boton pulsado");

         if (circuitItem != null)
         {
             pointerVR.beingHit = false;


             for (int i = 0; i < circuitManager.circuit.circuitElements.Count; i++)
             {
                 if (circuitManager.circuit.circuitElements[i].isSelected)
                 {
                     circuitManager.circuit.circuitElements[i].isSelected = false;
                     if (!circuitManager.circuit.circuitElements[i].ID.Contains("Valve"))
                         circuitManager.circuit.circuitElements[i].GetComponent<Outline>().enabled = false;
                     else
                         circuitManager.circuit.circuitElements[i].GetComponent<Valve>().ColorOutlineValve();
                 }
             }

             circuitManager.circuit.itemCircuitSelected = circuitItem;
             circuitManager.circuit.itemCircuitSelected.isSelected = true;
 #if UNITY_EDITOR
             AssetDatabase.Refresh();
 #endif
         }*/
    }
}