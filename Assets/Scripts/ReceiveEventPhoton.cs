using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class ReceiveEventPhoton : MonoBehaviour, IOnEventCallback
{
    Circuit circuit;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        circuit = GameObject.FindGameObjectWithTag("Circuit").GetComponent<Circuit>();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == 1)
            {
                object[] data = (object[])photonEvent.CustomData;
                Circuit aux = (Circuit)data[0];
                circuit = aux;
            }
        }
    }
        
}
