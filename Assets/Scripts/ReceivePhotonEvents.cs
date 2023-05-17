using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ReceivePhotonEvents : MonoBehaviour, IOnEventCallback
{
    const int ShareCircuitPlayer = 1;
    JsonReader jsonReader;
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
        jsonReader = FindObjectOfType<JsonReader>();
    }
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == ShareCircuitPlayer)
        {
            object[] data = (object[])photonEvent.CustomData;

            string tempData = (string)data[0];

            jsonReader.LoadTempFile(tempData);
        }
    }
}
