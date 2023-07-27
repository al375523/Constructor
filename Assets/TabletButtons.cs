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
    public List<GameObject> TypeHideList;
    public TMP_Text textMovement;
    public Transform rightController;
    public TMP_Text currentType;
    public ConstructionType type;
    int typeInt;

    OculusInputs controller;
    ConstructionPanel constructionPanel=null;    
    ProjectManagerDemo projectManager;
    string sceneName;
    GameObject tabletUbication;
    PhotonView pv;
    static bool hide = false;

    List<bool> hiddenObjects;

    void Start()
    {
        hiddenObjects = new List<bool>(new bool[TypeHideList.Count]);
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        rightController = GameObject.FindGameObjectWithTag("RightHand").transform;
        tabletUbication = GameObject.FindGameObjectWithTag("TabletUbication");
        projectManager = FindObjectOfType<ProjectManagerDemo>();
        pv = GetComponent<PhotonView>();
        sceneName = projectManager.sceneName;
        StartCoroutine(WaitArtScene());
        type = ConstructionType.Others;
        typeInt = 0;
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
        ChangePanel("MainPanel");
        constructionPanel = FindObjectOfType<ConstructionPanel>();
        hide = !hide;
        for (int i = 0; i < hiddenObjects.Count; i++)
            if (hiddenObjects[i])
            {
                TypeHideList[i].SetActive(true);
                typeInt = i;
                ChangeType(i);
                constructionPanel.HideAllItemsOfType(type);
            }
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
        if (!TypeHideList[typeInt].activeSelf)
        {
            hiddenObjects[typeInt] = true;
            constructionPanel.HideAllItemsOfType(type);
            TypeHideList[typeInt].SetActive(true);
        }
        else
        {
            hiddenObjects[typeInt] = false;
            constructionPanel.HideAllItemsOfType(type);
            TypeHideList[typeInt].SetActive(false);
        }
    }
  
    public void ChangeType(int t)
    {
        string sType="";
        
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
        typeInt = t;
        currentType.text = sType;
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
