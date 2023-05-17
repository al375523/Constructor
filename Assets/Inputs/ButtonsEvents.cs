using System;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Chiligames.MetaAvatarsPun;

public class ButtonsEvents : MonoBehaviour
{
    public CircuitManager circuitManager;
    public JsonReader jsonReader;
    public DisplayPanel displayPanel;
    public GameObject changeWaterPass;

    public SpawnPlayerButton playerButton;
    public GameObject controlButton;

    PlayerManager playerManager;
    OculusInputs oculusInputs;   

    [Header("UI TO SHOW OR HIDE")]
    [SerializeField] private GameObject rotateBtn;
    [SerializeField] private GameObject lengthBtn;
    [SerializeField] private GameObject valveWateCirculationBtn;
    [SerializeField] private GameObject editBtn;
    [SerializeField] private GameObject moveBtn;
    [SerializeField] private GameObject deleteBtn;
    [SerializeField] private GameObject completeBtn;
    [SerializeField] private GameObject createBtn;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private GameObject deletePanel;
    [SerializeField] private GameObject reductorChangeEndSizeBtn;
    [SerializeField] private GameObject valveButton;
    [SerializeField] private GameObject JSONPanel;
    [SerializeField] private PhotonView pv;

    void Awake()
    {
        
        oculusInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        circuitManager = GameObject.FindGameObjectWithTag("Circuit").GetComponent<CircuitManager>();
        jsonReader = GameObject.FindGameObjectWithTag("Reader").GetComponent<JsonReader>();
        jsonReader.LoadAllCircuitJsonFiles();
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
        pv = GetComponent<PhotonView>();
        if (jsonReader==null && circuitManager == null)
        {
            this.enabled = false;
        }
    }

  
    public void ButtonEvent(string n)
    {
        pv.RPC("ButtonSelected", RpcTarget.All, n);       
        //ButtonSelected(n);
    }

    [PunRPC]
    public void ButtonSelected(string n)
    {      
        switch (n)
        {
            case "Create":
                displayPanel.ShowDisplay(0);
                break;
            case "Edit":
                displayPanel.ShowDisplay(1);
                HideShowCanvas(circuitManager.circuit.itemCircuitSelected);
                break;
            case "Move":
                displayPanel.ShowDisplay(2);
                break;
            case "Eliminate":
                displayPanel.ShowDisplay(3);
                break;
            case "Complete":
                displayPanel.ShowDisplay(4);
                break;
            case "JSON":
                displayPanel.ShowDisplay(5);
                break;
            case "ChangePlayer":
                displayPanel.ShowDisplay(6);
                playerButton.SpawnButtons();
                break;
            case "HidePanels":
                displayPanel.HideAllPanels();
                break;
            case "Boiler":
                circuitManager.InstantiateItem("Boiler");
                break;
            case "Pump":
                circuitManager.InstantiateItem("Pump");
                break;
            case "Reductor":
                circuitManager.InstantiateItem("Reductor");
                break;
            case "Valve":
                circuitManager.InstantiateItem("Valve");
                break;
            case "Small Pipe":
                circuitManager.InstantiateItem("Small Pipe");
                break;
            case "Elbow":
                circuitManager.InstantiateItem("Elbow");
                break;
            case "Large Pipe":
                circuitManager.InstantiateItem("Large Pipe");
                break;
            case "Reset":
                circuitManager.ResetCircuit();
                break;
            case "Delete Last":
                EventManager.TriggerEvent("DESELECT_ITEM");
                circuitManager.DeleteLastItem();
                if (circuitManager.circuit.circuitElements.Count > 0)
                    completeBtn.SetActive(false);
                else if (circuitManager.circuit.circuitElements.Count > 2) completeBtn.SetActive(true);
                break;
            case "Deselect":
                circuitManager.DeselectItem();
                break;
            case "Delete Selected":
                var positionCircuit = circuitManager.circuit.itemCircuitSelected.positionInCircuit;
                if (positionCircuit != circuitManager.circuit.circuitElements.Count - 1 && positionCircuit != 0) completeBtn.SetActive(true);
                //EventManager.TriggerEvent("DESELECT_ITEM");               
                circuitManager.DeleteItem(circuitManager.circuit.itemCircuitSelected);
                break;
            case "RotateVertical":
                ((IEditableVerticalRotation)circuitManager.circuit.itemCircuitSelected).EditVerticalRotation(true);
                break;
            case "RotateHorizontal":
                ((IEditableHorizontalRotation)circuitManager.circuit.itemCircuitSelected).EditHorizontalRotation(true);
                break;
            case "RotateVerticalInPos":
                ((IEditableVerticalRotation)circuitManager.circuit.itemCircuitSelected).EditVerticalRotation(false);
                break;
            case "RotateHorizontalInPos":
                ((IEditableHorizontalRotation)circuitManager.circuit.itemCircuitSelected).EditHorizontalRotation(false);
                break;
            case "Save":
                //jsonReader.SaveToJson();
                break;
            case "LoadAll":
                //jsonReader.LoadAllCircuitJsonFiles();
                break;
            case "Sum001":
                EditLenght(0.01f);
                break;
            case "Rest001":
                EditLenght(-0.01f);
                break;
            case "Sum01":
                EditLenght(0.1f);
                break;
            case "Rest01":
                EditLenght(-0.1f);
                break;
            case "Sum1":
                EditLenght(1f);
                break;
            case "Rest1":
                EditLenght(-1f);
                break;
            case "ApplyMovement":
                circuitManager.ApplyMovementInSelectedItem();
                //playerManager.UpdateCircuit();
                break;
            case "MoveX001":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0.01f, 0, 0));
                break;
            case "MoveX01":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0.1f, 0, 0));
                break;
            case "MoveX1":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(1f, 0, 0));
                break;
            case "MoveY001":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0.01f, 0));
                break;
            case "MoveY01":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0.1f, 0));
                break;
            case "MoveY1":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 1f, 0));
                break;
            case "MoveZ001":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0, 0.01f));
                break;
            case "MoveZ01":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0, 0.1f));
                break;
            case "MoveZ1":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0, 1f));
                break;
            case "MoveX-001":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(-0.01f, 0, 0));
                break;
            case "MoveX-01":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(-0.1f, 0, 0));
                break;
            case "MoveX-1":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(-1f, 0, 0));
                break;
            case "MoveY-001":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, -0.01f, 0));
                break;
            case "MoveY-01":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, -0.1f, 0));
                break;
            case "MoveY-1":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, -1f, 0));
                break;
            case "MoveZ-001":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0, -0.01f));
                break;
            case "MoveZ-01":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0, -0.1f));
                break;
            case "MoveZ-1":
                ((IMoveable)circuitManager.circuit.itemCircuitSelected).Move(new Vector3(0, 0, -1f));
                break;
            case "Yes":
                print("completo el circuito");
                circuitManager.ConfigurateCircuitComplete(circuitManager.circuit.itemCircuitSelected, "NEXT");
                break;
            case "No":
                displayPanel.HideAllPanels();
                break;
            case "ChangePass":
                var circulation = (IEditableWaterCirculation)circuitManager.circuit.itemCircuitSelected;
                circulation.EditWaterCirculation();
                circuitManager.ChangeWaterPass();
                circuitManager.RefreshWaterPass();
                circuitManager.ChangeWaterPass();
                circuitManager.RefreshWaterPass();
                break;
            case "ShowWaterPass":
                print("ShowWaterPass");
                circuitManager.ChangeWaterPass();
                circuitManager.RefreshWaterPass();
                break;
            case "ChangeSize":
                print("ChangeSize");
                var item= circuitManager.circuit.itemCircuitSelected;
                var ieditableChangeSize = (IEditableChangeSize)item;
                ieditableChangeSize.EditChangeSize();
                break;
            case "SaveJSON":
                jsonReader.SaveToJson();
                break;
            case "LoadJSON":
                jsonReader.LoadCurrentJsonFile();
                break;
            case "ChangeSave":
                jsonReader.saveToNew = !jsonReader.saveToNew;
                break;
            default:
                Debug.Log("Not implemented yet");
                break;
         
        }
        if(PhotonNetwork.IsMasterClient)
            jsonReader.SaveToJsonTemp();
    }
    public void EditLenght(float delta) {
        ((IEditableLength)circuitManager.circuit.itemCircuitSelected).EditLength(delta);
        if (circuitManager.circuit.circuitElements.Count <= 1) return;
        circuitManager.ReubicateObject(circuitManager.circuit.itemCircuitSelected.prevItem, circuitManager.circuit.itemCircuitSelected);
        //playerManager.UpdateCircuit();
    }

    /// <summary>
    /// Depending of ID, edit will show some or other info
    /// </summary>
    /// <param name="id">Id of item selected</param>
    public void HideShowCanvas(CircuitItem item)
    {
        HideAllEditableButtons();

        var editLenght = item as IEditableLength;
        if (editLenght!=null && editLenght.CanEditLength()) lengthBtn.SetActive(true);

        var editableHorizontal = item as IEditableHorizontalRotation;
        if (editableHorizontal != null && editableHorizontal.CanEditHorizontalRotation()) rotateBtn.SetActive(true);

        var editCirculation = item as IEditableWaterCirculation;
        if (editCirculation != null && editCirculation.CanEditWaterCirculation()) valveWateCirculationBtn.SetActive(true);

        var editSize = item as IEditableChangeSize;
        if (editSize != null && editSize.CanEditChangeSize()) reductorChangeEndSizeBtn.SetActive(true);
    }

    private void HideAllEditableButtons()
    {
        lengthBtn.SetActive(false);
        rotateBtn.SetActive(false);
        valveWateCirculationBtn.SetActive(false);
        reductorChangeEndSizeBtn.SetActive(false);
    }

    private void Update()
    {
        ShowCreateBtn();
        ShowEditBtn();
        ShowMoveBtn();
        ShowDeleteBtn();
        ShowCompleteBtn();
        ShowValveBtn();

        if (PhotonNetwork.IsMasterClient)
        {
            controlButton.SetActive(true);            
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            controlButton.SetActive(false);           
        }
    }
    #region ShowBtn
    private void ShowCompleteBtn()
    {
        if (circuitManager.circuit.circuitElements.Count < 2) completeBtn.SetActive(false);
    }

    private void ShowValveBtn()
    {
        if (circuitManager.circuit.circuitElements.Count < 1) valveButton.SetActive(false);
        else valveButton.SetActive(true);
    }

    private void ShowDeleteBtn()
    {
        if (circuitManager.circuit.circuitElements.Count > 0) deleteBtn.SetActive(true);
        else deleteBtn.SetActive(false);
    }

    private void ShowCreateBtn()
    {
        if (circuitManager.circuit.itemCircuitSelected != null && circuitManager.circuit.itemCircuitSelected.nextItem != null)
        {
            createBtn.SetActive(false);
            createPanel.SetActive(false);
        }
        else
        {
            createBtn.SetActive(true);
        }
    }

    private void ShowEditBtn()
    {
        if (circuitManager.circuit.itemCircuitSelected != null) editBtn.SetActive(true);
        else editBtn.SetActive(false);
    }

    private void ShowMoveBtn()
    {
        if (circuitManager.circuit.itemCircuitSelected != null)
        {
            var moveable = circuitManager.circuit.itemCircuitSelected as IMoveable;
            if (moveable != null) moveBtn.SetActive(true);
            else moveBtn.SetActive(false);
        }
        else moveBtn.SetActive(false);
    }
    #endregion

}