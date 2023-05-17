using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayerButton : MonoBehaviour
{
    public GameObject buttonPrefab;

    ButtonsEvents buttonsEvents;
    float nPlayers;
    OculusInputs oculusInputs;
    bool alreadySpawned;
    // Start is called before the first frame update
    void Start()
    {
        oculusInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        alreadySpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnButtons()
    {
        if (alreadySpawned)
        {
            GameObject[] children = GetComponentsInChildren<GameObject>();
            while (children.Length > 0)
            {
                Destroy(children[0]);
            }
        }
        
        GameObject button = null;
        nPlayers = PhotonNetwork.CountOfPlayers;
        for(int i=1; i<=nPlayers; i++)
        {
            if (i != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                button = Instantiate(buttonPrefab, transform);
                button.GetComponent<ChangeControl>().playerNumber = i;
                button.GetComponent<TMPro.TMP_Text>().text = "Player " + i;
            }
        }
        if(button!=null)
            alreadySpawned = true;
    }

    public void ChangeControl(int n)
    {        
        PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[n]);
        buttonsEvents.ButtonEvent("HidePanels");
    }


}
