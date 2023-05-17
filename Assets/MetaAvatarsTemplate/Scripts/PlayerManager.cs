using ExitGames.Client.Photon;
using Oculus.Avatar2;
using Oculus.Platform;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//
//For handling local objects and sending data over the network
//
namespace Chiligames.MetaAvatarsPun
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        [SerializeField] GameObject OVRCameraRig;
        [SerializeField] Transform centerEyeAnchor;
        [SerializeField] Transform[] spawnPoints;

        [SerializeField] GameObject avatarPrefab;
        [SerializeField] GameObject photonVoiceSetupPrefab;
        [SerializeField] GameObject circuitPrefab;

        [HideInInspector] public ulong userID = 0;

        public const byte UpdateCircuitEvent = 1;

        bool userIsEntitled = false;
        private TMP_Text text;
        [HideInInspector] public GameObject avatarObject;
        Circuit circuit;
        PhotonView pv;
        JsonReader jsonReader;
        ProjectManagerDemo projectManager;
        string sceneName;
        GameObject cameraCenter;
        private void Awake()
        {
            pv = GetComponent<PhotonView>();
            jsonReader = FindObjectOfType<JsonReader>();
            SetPlayerPosition();
            projectManager = FindObjectOfType<ProjectManagerDemo>();
            cameraCenter = GameObject.FindGameObjectWithTag("MainCamera");
            sceneName = projectManager.sceneName;
            //text = GameObject.Find("TextError").GetComponent<TMPro.TMP_Text>();
            
        }

        private void Start()
        {
           
        }

        private void Update()
        {
            if (avatarObject != null)
            {
                avatarObject.transform.position = new Vector3(cameraCenter.transform.position.x, 
                                                                OVRCameraRig.transform.position.y, 
                                                                cameraCenter.transform.position.z);
            }
        }

        public void ConnectToServer()
        {
            //Initialize the oculus platform
            try
            {
                Core.AsyncInitialize();
                Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
            }
            catch (UnityException e)
            {
                Debug.LogError("Platform failed to initialize due to exception.");
                Debug.LogException(e);
                //text.text += "AwakePlayerManager" + e.Message + "\n";
            }
        }

        void EntitlementCallback(Message msg)
        {
            if (msg.IsError)
            {
                Debug.LogError("You are NOT entitled to use this app. Please check if you added the correct ID's and credentials in Oculus>Platform");
                //text.text += "You are NOT entitled to use this app. Please check if you added the correct ID's and credentials in Oculus>Platform" + "\n";
                //UnityEngine.Application.Quit();
            }
            else
            {
                Debug.Log("You are entitled to use this app.");
                //text.text += "You are entitled to use this app." + "\n";
                GetTokens();
            }
        }

        public override void OnJoinedRoom()
        {
            //Set the user to different spawning locations
            if (PhotonNetwork.LocalPlayer.ActorNumber <= spawnPoints.Length)
            {
                OVRCameraRig.transform.position = new Vector3(spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position.x,
                    OVRCameraRig.transform.position.y,
                    spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position.z);
                OVRCameraRig.transform.rotation = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation;                             
            }           
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //jsonReader.ShareCircuit(newPlayer);
            }
        }

        //Get Access token and user ID from Oculus Platform
        private void GetTokens()
        {

            Users.GetAccessToken().OnComplete(message =>
            {
                if (!message.IsError)
                {
                    OvrAvatarEntitlement.SetAccessToken(message.Data);
                    Users.GetLoggedInUser().OnComplete(message =>
                    {
                        if (!message.IsError)
                        {
                            userID = message.Data.ID;
                            userIsEntitled = true;
                            StartCoroutine(SpawnAvatar());
                        }
                        else
                        {
                            //text.text +="SetAccessToken Fallo "+ message.GetError() + "\n"; 
                            var e = message.GetError();
                        }
                    });
                }
                else
                {
                    //text.text += "User GetAccessToken fallo "+ message.GetError() + "\n";
                    var e = message.GetError();
                }
            });
        }

        IEnumerator SpawnAvatar()
        {

            //Wait for all the entitlements and the runner to be ready to spawn
            while (!userIsEntitled || !OvrAvatarEntitlement.AccessTokenIsValid)
            {
                yield return null;
            }
            yield return new WaitForSeconds(3f);  
            try
            {
                //Avatar spawning and seting its parent to be the OVR Camera Rig.
                var avatarEntity = PhotonNetwork.Instantiate(avatarPrefab.name, OVRCameraRig.transform.position, OVRCameraRig.transform.rotation);
                avatarEntity.transform.SetParent(GameObject.FindGameObjectWithTag("Tracking").transform);
                avatarEntity.layer = 5;               
                avatarEntity.transform.localRotation = Quaternion.identity;
                //Send an rpc with the Oculus UserID so other users can access to it and load our avatar.
                avatarEntity.GetComponent<PhotonView>().RPC("RPC_SetOculusID", RpcTarget.AllBuffered, (long)userID);
                //Instantiate the voice setup and set it as a child of the center eye anchor (head).
                var voiceSetup = PhotonNetwork.Instantiate(photonVoiceSetupPrefab.name, centerEyeAnchor.transform.position, centerEyeAnchor.transform.rotation);
                voiceSetup.transform.SetParent(centerEyeAnchor);
                voiceSetup.transform.localPosition = Vector3.zero;
                voiceSetup.transform.localRotation = Quaternion.identity;

                //Lipsync setup for our avatar
                avatarEntity.GetComponent<PunAvatarEntity>().SetLipSync(voiceSetup.GetComponent<OvrAvatarLipSyncContext>());
                voiceSetup.GetComponent<OvrAvatarLipSyncContext>().CaptureAudio = true;
                avatarObject = avatarEntity;
                //PhotonNetwork.LeaveRoom();
            }
            catch (Exception e)
            {
                Debug.Log("SpawnAvatar Fallo "+ e.Message+"\n");
            }
            //pv.RPC("UpdateCircuitOwner", RpcTarget.All);           
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //pv.RPC("UpdateCircuitOwner", RpcTarget.All);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            //pv.RPC("UpdateCircuitOwner", RpcTarget.All);
        }

        [PunRPC]
        public void UpdateCircuitOwner()
        {
            //UpdateCircuit();
            if(!PhotonNetwork.IsMasterClient)
                circuit.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
        }
       
        public void UpdateCircuit()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                object[] content = new object[] {circuit};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
                PhotonNetwork.RaiseEvent(UpdateCircuitEvent, content, raiseEventOptions, SendOptions.SendReliable);
            }            
        }  
        
        public void SetPlayerPosition()
        {
            StartCoroutine(WaitArtScene());
        }
        IEnumerator WaitArtScene()
        {
            yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
            GetPositions();            
            //circuit = GameObject.FindGameObjectWithTag("Circuit").GetComponent<Circuit>();
        }

        public void GetPositions()
        {
            Transform[] positions = GameObject.FindGameObjectWithTag("PlayerPositions").GetComponentsInChildren<Transform>();
            if (!PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.ActorNumber <= positions.Length)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber > 0) SetNewPosition(positions[PhotonNetwork.LocalPlayer.ActorNumber - 1]);
                else SetNewPosition(positions[0]);
            }
            else
            {
                SetNewPosition(positions[0]);
            }
                
        }

        void SetNewPosition(Transform pos)
        {
            OVRCameraRig.transform.position = new Vector3(pos.transform.position.x, projectManager.actualY, pos.transform.position.z);
            OVRCameraRig.transform.rotation = pos.transform.rotation;
        }

        
    }
}
