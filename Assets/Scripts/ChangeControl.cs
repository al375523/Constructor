using Chiligames.MetaAvatarsPun;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControl : MonoBehaviour
{
    public int playerNumber = -1;
    SpawnPlayerButton playerButtons;
    PlayerManager playerManager;
    OculusInputs oculusInputs;
    
    // Start is called before the first frame update
    void Start()
    {
        oculusInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<OculusInputs>();
        playerButtons = gameObject.GetComponentInParent<SpawnPlayerButton>();
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
    }

    public void GiveControl()
    {
        if (PhotonNetwork.IsMasterClient)
            //playerManager.UpdateCircuit();
        playerButtons.ChangeControl(playerNumber);
    }
}
