using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static OVRInput;
using Chiligames.MetaAvatarsPun;
using UnityEngine.SceneManagement;

public class OculusInputs : MonoBehaviour
{
    [HideInInspector]
    public GameObject tabletPrefab = null;

    public PlayerManager playerManager;
    public Controller leftController;
    public Controller rightController;
    public GameObject uiToShow = null;    
    public Vector2 joystickR;
    public Vector2 joystick;
    

    public GameObject minimapPrefab = null;
    public Transform minimapPosition;

    

    public LayerMask LimitLayer;
    public GameObject cameraParent;
    public bool minimapOpened = false;
    public GameObject loadingScreen;
    public GameObject teleportObject;

    PointerVR pointer;
    float leftAxisHand; 
    float leftAxisIndex;
    float rightAxisHand;
    float speed = 2f;
    bool selectButton; 
    bool isPressed;
    bool holding;
    bool holdingSelected;
    bool holdingMinimap;
    bool rotate;

    MenuManager submenuManager;
    GameObject canvasPos;
    GameObject mainCamera;
    Animator animator;
    Vector2 joystickL;
    LineRenderer pointerRender;
    public enum InputStates
    {
        Inactive,
        Active,
        MenuOpened,
        MinimapSubmenu
    }

    public InputStates CurrentInputState { get; private set; }

    public enum MovementStates
    {
        Walk,
        Fly
    }

    public MovementStates CurrentMovementState { get; private set; }
    
    private void OnEnable()
    {
        CurrentInputState = InputStates.MenuOpened;
        CurrentMovementState = MovementStates.Walk;
    }

    void Start()
    {        
        isPressed = false;
        holdingSelected = false;
        holdingMinimap = false;
        rotate = true;
        mainCamera = Camera.main.gameObject;
        cameraParent = mainCamera.transform.parent.parent.gameObject;
        canvasPos = GameObject.FindGameObjectWithTag("Ubication");       
        pointer = GetComponentInChildren<PointerVR>();
        submenuManager = uiToShow.GetComponent<MenuManager>();       
        EventManager.StartListening("LOADING_SCREEN", LoadingScreen);
        pointerRender = GetComponentInChildren<LineRenderer>();
        animator = GetComponentInChildren<Animator>();
    }
    

    void FixedUpdate()
    {         
        CheckInputs();
        JoysticksFuncionalities();
    }

    void Update()
    {       
        ButtonsFuncionalities();
        uiToShow.transform.LookAt(Camera.main.transform.position);
        minimapPrefab.transform.LookAt(Camera.main.transform.position);
    }

    void CheckInputs()
    {
        if (CurrentInputState == InputStates.Active)
        {
            selectButton = GetDown(Button.One, leftController);
            leftAxisHand = Get(Axis1D.PrimaryHandTrigger, leftController);
            rightAxisHand = Get(Axis1D.PrimaryHandTrigger, rightController);
            joystickR = Get(Axis2D.PrimaryThumbstick, rightController);
            joystickL = Get(Axis2D.PrimaryThumbstick, leftController);
            leftAxisIndex = Get(Axis1D.PrimaryIndexTrigger, leftController);
            pointer.enabled = true;
            pointerRender.enabled = true;
            teleportObject.SetActive(true);
            if (tabletPrefab != null) tabletPrefab.SetActive(true);
        }
        else if(CurrentInputState == InputStates.Inactive)
        {
            selectButton = false;
            leftAxisHand = 0;
            rightAxisHand = 0;
            joystickL = Vector2.zero;
            joystickR = Vector2.zero;
            leftAxisIndex = 0;
            pointer.enabled = false;
            pointerRender.enabled = false;
            teleportObject.SetActive(false);
            if (tabletPrefab != null) tabletPrefab.SetActive(false);
        }
        else if (CurrentInputState == InputStates.MenuOpened)
        {
            joystickL = Vector2.zero;
            joystickR = Vector2.zero;
            teleportObject.SetActive(false);
            selectButton = false;
            pointer.enabled = true;
            pointerRender.enabled = true;
            leftAxisIndex = Get(Axis1D.PrimaryIndexTrigger, leftController);
            if (tabletPrefab != null) tabletPrefab.SetActive(false);
        }
        else if(CurrentInputState == InputStates.MinimapSubmenu)
        {
            rightAxisHand = Get(Axis1D.PrimaryHandTrigger, rightController);
            leftAxisHand = Get(Axis1D.PrimaryHandTrigger, leftController);
            leftAxisIndex = Get(Axis1D.PrimaryIndexTrigger, leftController);
            pointer.enabled = true;
            pointerRender.enabled = true;
            joystickL = Vector2.zero;
            joystickR = Vector2.zero;
            teleportObject.SetActive(false);
            selectButton = false;
            if (tabletPrefab != null) tabletPrefab.SetActive(false);
        }
    }

    void JoysticksFuncionalities()
    {
        //FixedUpdate
        //Movement
        if (CurrentInputState != InputStates.MenuOpened && CurrentInputState != InputStates.MinimapSubmenu)
        {
            if (joystickL != Vector2.zero)
            {               
                Move();                
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }

            //Rotation
            if (joystickR != Vector2.zero && rotate)
            {
                Rotate();
                rotate = false;
                StartCoroutine(CooldownRotation());
            }
        }
    }

    void ButtonsFuncionalities()
    {            
    //Update
       //Select A Button
        if (selectButton && !holdingSelected)
        {
            holdingSelected = true;
            
        }
        else
        {
            holdingSelected = false;
        }

       //Show Submenu
        if (leftAxisHand > 0 && !minimapPrefab.activeSelf && !isPressed)
        {
            ShowHideUI();
            isPressed = true;
        }
        else if(leftAxisHand == 0)
        {
            isPressed = false;
        }

       //Pointer select
        if (leftAxisIndex > 0 && !holding)
        {
            pointer.ButtonPressed();
            holding = true;
        }
        else
        {
            pointer.ButtonReleased();
            holding = false;
        }

       //Minimap
        if (rightAxisHand > 0 && !holdingMinimap && !uiToShow.activeSelf)
        {
            OpenCloseMinimap();
            holdingMinimap = true;
        }
        else if(rightAxisHand == 0)
        {
            holdingMinimap = false;
        }
    }


    public void ChangeInputState(int i)
    {
        switch (i)
        {
            default:
                break;
            case 0:
                CurrentInputState = InputStates.Inactive;
                break;
            case 1:
                CurrentInputState = InputStates.Active;
                break;
            case 2:
                CurrentInputState = InputStates.MenuOpened;
                break;
            case 3:
                CurrentInputState = InputStates.MinimapSubmenu;
                break;
        }
        Debug.Log("State changed to: " + i);
    }

    public int GetInputState()
    {
        return ((int)CurrentInputState);
    }

    public void ChangeMovementState(int i)
    {
        switch (i)
        {
            case 0:
                CurrentMovementState = MovementStates.Walk;
                break;
            case 1:
                CurrentMovementState = MovementStates.Fly;
                break;
        }
    }

    void ShowHideUI()
    {                           
        if (!uiToShow.activeSelf)
        {
           uiToShow.SetActive(true);          
           ChangeInputState(3);
        }
        else
        {           
           ChangeInputState(1);
           submenuManager.ChangePanel(0);
           uiToShow.SetActive(false);
        }
        uiToShow.transform.position = new Vector3(canvasPos.transform.position.x, canvasPos.transform.position.y, canvasPos.transform.position.z);
    }

    void Rotate()
    {
        Vector2 r = new Vector2(0, 0);
        if (joystickR.x >= 0.1f)
        {
            r = new Vector2(0, 45);
        }
        else if (joystickR.x <= -0.1f)
        {
            r = new Vector2(0, -45);
        }

        transform.Rotate(r, Space.World);
       
    }

    public void Move()
    {
        float x = joystickL.x;
        float y = joystickL.y;
        Vector3 movement = Vector3.zero;

        //Is flying
        if (CurrentMovementState == MovementStates.Fly) 
        {           
            Vector3 vectorForward = mainCamera.transform.forward;
            Vector3 vectorRight = mainCamera.transform.right;
            movement = ((vectorRight * x + vectorForward * y).normalized) * Time.deltaTime * speed;                
        }

        //Is walking 
        else if(CurrentMovementState == MovementStates.Walk)
        {
            animator.SetBool("IsWalking", true);
            Vector3 vectorForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
            Vector3 vectorRight = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);
            movement = (vectorRight * x + vectorForward * y).normalized * Time.deltaTime * (speed-0.5f);            
        }

        if (CheckValidMovement(movement))
        {
            cameraParent.transform.position += movement;
        }
        else
        {
            Debug.Log("Fuera de los límites");
        }
    }

    

    bool CheckValidMovement(Vector3 movement)
    {
        Vector3 aux = cameraParent.transform.position;
        Collider[] colliders = Physics.OverlapBox(aux + movement, new Vector3(0.2f,0.3f,0.2f), Quaternion.identity, LimitLayer);
        if (colliders.Length == 0)
        {
            return true;
        }
        return false;        
    }

    IEnumerator CooldownRotation()
    {
        yield return new WaitForSeconds(0.2f);
        rotate = true;
    }
      
    public void ResetPositionY()
    {       
        playerManager.GetPositions();
        ResetRotation();
    }

    void ResetRotation()
    {
        cameraParent.transform.rotation = Quaternion.identity;
        
    }

    public void OpenCloseMinimap()
    {               
       minimapPrefab.SetActive(!minimapOpened);
        if (minimapPrefab.activeSelf)
        {
            ChangeInputState(3);
            teleportObject.SetActive(false);
            minimapPrefab.transform.position = new Vector3(canvasPos.transform.position.x, canvasPos.transform.position.y, canvasPos.transform.position.z);
        }
        else
        {
            ChangeInputState(1);
            teleportObject.SetActive(true);
        }
        minimapOpened = !minimapOpened;             
    }

    public bool GetSelect()
    {
        return selectButton;
    }    

    void LoadingScreen()
    {
        loadingScreen.SetActive(!loadingScreen.activeSelf);

        if (loadingScreen.activeSelf)
        {
            ChangeInputState(0);
        }

        else 
        {
            ChangeInputState(1);
        }        
    }  
}
