using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
//
//This script connects to PHOTON servers and creates a room if there is none, then automatically joins
//
namespace Chiligames.MetaAvatarsPun
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager instance;
        public MenuManager menuManager;

        private TMP_Text text;
        PlayerManager playerManager;
        PhotonView pv;
        
        public enum NetworkStates
        {
            Disconnected,
            InMaster,
            InRoom,
        }

        public NetworkStates CurrentNetworkState { get; private set; }

        private void Awake()
        {
            CurrentNetworkState = NetworkStates.Disconnected;
            playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();            
            //text = GameObject.Find("TextError").GetComponent<TMPro.TMP_Text>();
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
           
        }

        private void Update()
        {
            if (!PhotonNetwork.IsConnected)
            {
                CurrentNetworkState = NetworkStates.Disconnected;
            }
        }

        public void ConnectToMaster()
        {
            try
            {
                PhotonNetwork.OfflineMode = false; //true would "fake" an online connection
                PhotonNetwork.NickName = "PlayerName"; //we can use a input to change this 
                PhotonNetwork.AutomaticallySyncScene = true; //To call PhotonNetwork.LoadLevel()
                PhotonNetwork.GameVersion = "v1"; //only people with the same game version can play together
                PhotonNetwork.UseRpcMonoBehaviourCache = true;

                //PhotonNetwork.ConnectToMaster(ip, port, appid); //manual connection
                PhotonNetwork.ConnectUsingSettings(); //automatic connection based on the config file
            }
            catch (System.Exception e)
            {                 
                text.text += "Photon ConnectToMaster" + e.Message +"\n";
            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log(cause);
            CurrentNetworkState = NetworkStates.Disconnected;
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();                        
            Debug.Log("Connected to master!");
            CurrentNetworkState = NetworkStates.InMaster;
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            CurrentNetworkState = NetworkStates.InMaster;
            //Destroy(playerManager.avatarObject);
        }

        public void ConnectToRoom()
        {
            if (!PhotonNetwork.IsConnected)
            {
                //text.text += "Photon Network isn't connected";
                Debug.Log("Photon Network isn't connected");
                return;
            }

            RoomOptions roomOptions = new RoomOptions();
            //Room max is set to 10, as there are 10 spawning point locations. Max Pun2 FREE amount of users in a room can be set to 20.
            roomOptions.MaxPlayers = 4;
            //The name of the room can be changed here, or randomized.
            try
            {
                //PhotonNetwork.CreateRoom("MetaAvatars", roomOptions, TypedLobby.Default);
                PhotonNetwork.JoinOrCreateRoom("MetaAvatars", roomOptions, TypedLobby.Default);
            }
            catch (System.Exception e)
            {
                //text.text += "ConnectToRoom" + e.Message+ "\n";
                Debug.Log("ConnectToRoom" + e.Message);
            }

        }
            
        public void JoinRoom()
        {
            if (!PhotonNetwork.IsConnected)
            {
                //text.text += "Photon Network isn't connected";
                Debug.Log("Photon Network isn't connected");
                return;
            }
            try
            {
                PhotonNetwork.JoinRoom("MetaAvatars");
                //PhotonNetwork.JoinRandomRoom();
            }
            catch (System.Exception e)
            {
                Debug.Log("An error occurred. Error: " + e);
            }
        }

        public void LeaveRoom()
        {
            if (!PhotonNetwork.IsConnected)
            {
                //text.text += "Photon Network isn't connected";
                Debug.Log("Photon Network isn't connected");
                return;
            }
            try { 
                PhotonNetwork.LeaveRoom();
                menuManager.ChangeInfoText("Left Room");
            }
            catch (System.Exception e) { Debug.Log("An error occurred. Error: " + e); }

        }

        public void DisconnectFromMaster()
        {
            if (!PhotonNetwork.IsConnected)
            {
                //text.text += "Photon Network isn't connected";
                Debug.Log("Photon Network isn't connected");
                return;
            }
            try { 
                PhotonNetwork.Disconnect();
                menuManager.ChangeInfoText("Disconnected from Server!");
            }
            catch (System.Exception e) { Debug.Log("An error occurred. Error: " + e); }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);
            //text.text += "Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion+"\n";
            CurrentNetworkState = NetworkStates.InRoom;
            menuManager.ChangeInfoText("Joined to Room!");
            playerManager.ConnectToServer();
            StartCoroutine(WaitLoading());
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log("Ha fallado");
            ConnectToRoom();            
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            menuManager.ChangeInfoText("Room created!");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);            
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
        }

        
        IEnumerator WaitLoading()
        {
            EventManager.TriggerEvent("LOADING_SCREEN");
            yield return new WaitForSeconds(10f);                                             
            EventManager.TriggerEvent("LOADING_SCREEN");
            GameObject avatarLOD = GameObject.Find("LOD0");
            if (avatarLOD != null)
            {
                foreach (Transform tr in avatarLOD.GetComponentsInChildren<Transform>())
                {
                    tr.gameObject.layer = 5;
                }
            }
            playerManager.avatarObject.transform.rotation = Camera.main.transform.rotation;
        }
    }
}