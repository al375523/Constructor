using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Chiligames.MetaAvatarsPun;
using static Chiligames.MetaAvatarsPun.NetworkManager;

public class MenuManager : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Quaternion initialRotation;
    [HideInInspector] public float addedHeight;

    public bool isSubmenu;
    public List<GameObject> panels;   
    public GameObject actualPanel;    
    public TMP_Text multiplayerText;        
    [Tooltip("Room Options Buttons")]
    public List<GameObject> multiplayerButtons;
    [Tooltip("Give info about the actual state of the connection")]
    public GameObject panelText;
    public TMP_Text infoText;
    GameObject player;
    ProjectManagerDemo projectManager;
    //NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        projectManager = FindObjectOfType<ProjectManagerDemo>();
        actualPanel = panels[0];
        initialPosition = Vector3.zero;
        initialRotation = Quaternion.identity;
        player = GameObject.FindGameObjectWithTag("Player");       
        addedHeight = 0;
        //if (isSubmenu) networkManager = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
        StartCoroutine(WaitSeconds());
    }
    
    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(5f);
        if (gameObject.name.Contains("Main"))
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (name.Contains("Main"))
        {
            transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        }
        /*if (isSubmenu)
        {            
            ShowButtons();
 
        }*/
    }

    public void ChangePanel(int index)
    {
        actualPanel.SetActive(false);
        panels[index].SetActive(true);
        actualPanel = panels[index];
    }   

    public void PlayScene(string name)
    {
        projectManager.StartSection(name);
    }

    public void ResetMenu()
    {
        ChangePanel(0);      
    }      
    public void ExitApp(bool v)
    {
        //Menu confirmacion
        if (v)
        {
            Application.Quit();
        }              
    }

    public void VolumeUp()
    {
        if(AudioListener.volume < 1)
            AudioListener.volume += 0.1f;        
    }


    public void VolumeDown()
    {
        if (AudioListener.volume > 0)
            AudioListener.volume -= 0.1f;
    }
    
    public void SetHeight(float h)
    {
        if (h == 0)
        {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        }
        else
        {
            addedHeight += h;
            if (addedHeight > -1f && addedHeight < 1f)
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + h, player.transform.position.z);                
            }      
        }
    }

    #region Multiplayer Actions
    public void CreateRoom()
    {
        try 
        { 
            //networkManager.ConnectToRoom(); 
        }
        catch(Exception e)
        {           
            string m = "Error creating room: " + e;
            ChangeInfoText(m);
        }
    }

    public void JoinRoom()
    {         
        try
        {
            //networkManager.JoinRoom();
        }
        catch (Exception e)
        {
            string m = "Error joining room: " + e;
            ChangeInfoText(m);
        }
    }

    public void Disconnect()
    {        
        try
        {
            //networkManager.DisconnectFromMaster();
        }
        catch (Exception e)
        {
            string m = "Error disconnecting from master: " + e;
            ChangeInfoText(m);
        }
    }

    public void LeaveRoom()
    {       
        try
        {
            //networkManager.LeaveRoom();
        }
        catch (Exception e)
        {
            string m = "Error leaving room: " + e;
            ChangeInfoText(m);
        }
    }

    public void ConnectToMaster()
    {        
        try
        {
            //networkManager.ConnectToMaster();
        }
        catch (Exception e)
        {
            string m = "Error connecting to master: " + e;
            ChangeInfoText(m);
        }
        
    }
    #endregion

    #region Multiplayer Buttons

    public void ShowButtons()
    {
        /*foreach(GameObject gb in multiplayerButtons)
        {
            gb.SetActive(false);
        }*/

        //yield return new WaitForSeconds(2f);
        /*if (networkManager.CurrentNetworkState == NetworkManager.NetworkStates.Disconnected)
        {
            multiplayerButtons[3].SetActive(true);
            multiplayerText.text = "Connect to Server:";
        }
        else if (networkManager.CurrentNetworkState == NetworkManager.NetworkStates.InMaster)
        {
            multiplayerButtons[0].SetActive(true);
            multiplayerButtons[1].SetActive(true);
            multiplayerButtons[4].SetActive(true);
            multiplayerText.text = "Room Options:";
        }
        else if (networkManager.CurrentNetworkState == NetworkManager.NetworkStates.InRoom)
        {
            multiplayerButtons[2].SetActive(true);
            multiplayerButtons[4].SetActive(true);
            multiplayerText.text = "Room Options:";
        }*/
    }
    #endregion

    #region Info text
    public void ChangeInfoText(string message)
    {
        infoText.text = message;
        StartCoroutine(PanelInfo());
    }
    IEnumerator PanelInfo()
    {
        panelText.SetActive(true);
        yield return new WaitForSeconds(2f);
        panelText.SetActive(false);
    }
    #endregion
}
