using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class TabletButtons : MonoBehaviour
{        
    public List<GameObject> PanelList;
    public TMP_Text textMovement;
    public Transform rightController;
    public TMP_Text currentType;
    public ConstructionType type;

    OculusInputs controller;
    ConstructionPanel constructionPanel=null;    
    ProjectManagerDemo projectManager;
    string sceneName;
    GameObject tabletUbication;
    PhotonView pv;
    static bool hide = false;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        rightController = GameObject.FindGameObjectWithTag("RightHand").transform;
        tabletUbication = GameObject.FindGameObjectWithTag("TabletUbication");
        projectManager = FindObjectOfType<ProjectManagerDemo>();
        pv = GetComponent<PhotonView>();
        sceneName = projectManager.sceneName;
        StartCoroutine(WaitArtScene());
        type = ConstructionType.Others;
        currentType.text="Others";
        textMovement.text = "Fly";
        EventManager.StartListening("LOADING_SECTION", LoadingSection);
    }
    void Update()
    {        
        transform.position = tabletUbication.transform.position;
    }
    
    void LoadingSection()
    {
        constructionPanel = FindObjectOfType<ConstructionPanel>();
        hide = !hide;
        ShowHide();
    }

    IEnumerator WaitArtScene()
    {
        yield return new WaitUntil(()=>SceneManager.GetSceneByName(sceneName).isLoaded);
        constructionPanel = FindObjectOfType<ConstructionPanel>();
        controller = FindObjectOfType<OculusInputs>();
        //controller.playerY = controller.cameraParent.transform.position.y;
    }
    
    public void ShowHide()
    {
        if (!hide)
        {
            constructionPanel.ShowHideAllItemsOfType(type);
            hide = true;
        }
        else
        {
            constructionPanel.ShowAllItems();
            hide = false;
        }      
    }
  
    public void ChangeType(int t)
    {        
        string sType="";
        if(hide) ShowHide();
        switch (t)
        {
            case 0:
                type = ConstructionType.Others;
                sType = "Others";
                break;
            case 1:
                type = ConstructionType.Structure;
                sType = "Structure";
                break;
            case 2:
                type = ConstructionType.Equipment;
                sType = "Equipment";
                break;
            case 3:
                type = ConstructionType.Ceiling;
                sType = "Ceiling";
                break;
            case 4:
                type = ConstructionType.Plants;
                sType = "Plants";
                break;
            case 5:
                type = ConstructionType.People;
                sType = "People";
                break;
            case 6:
                type = ConstructionType.Decoration;
                sType = "Decoration";
                break;
        }
        currentType.text = sType;
        ChangePanel("");
        StartCoroutine(WaitPanel("MainPanel"));
    }

    

    public void ChangeToTypePanel()
    {        
        ChangePanel("");
        StartCoroutine(WaitPanel("ShowHidePanel"));
    }
    
    public void ChangePanel(string panelName)
    {        
        foreach (GameObject panel in PanelList)
        {
            if (panel.name != panelName) panel.SetActive(false);           
            else panel.SetActive(true);
        }
    }

    IEnumerator WaitPanel(string n)
    {
        yield return new WaitForSeconds(1f);
        ChangePanel(n);
    }

    public void ChangeMovement()
    {
        if (controller.CurrentMovementState == 0)
        {
            controller.ChangeMovementState(1);           
            textMovement.text = "Walk";           
        }
        else 
        {            
            textMovement.text = "Fly";            
            controller.ResetPositionY();
            controller.ChangeMovementState(0);
        }
    }    
}
