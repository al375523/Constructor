using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;

namespace Chiligames.MetaAvatarsPun
{
    public class PunAvatarEntity : OvrAvatarEntity
    {
        [SerializeField] StreamLOD streamLOD = StreamLOD.Medium;
        private PhotonView _photonView;
        [SerializeField] List<byte[]> _streamedDataArray = new List<byte[]>();

        private int _maxBytesToLog = 5;

        [SerializeField] float _intervalToSendData = 0.08f;
        private float _cycleStartTime = 0;

        private bool skeletonLoaded = false;
        private bool userIDSet;

        protected override void Awake()
        {
        }

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            ConfigureAvatar();
            base.Awake();
            //After entity is created, we can set the remote avatar to be third person (and have a head!)
            if (!_photonView.IsMine)
            {
                SetActiveView(CAPI.ovrAvatar2EntityViewFlags.ThirdPerson);
            }
            StartCoroutine(TryToLoadUser());
        }

        //Procedurally set the avatar creation features, this needs to be done before base.Awake() to be effective.
        void ConfigureAvatar()
        {
            if (_photonView.IsMine)
            {
                SetIsLocal(true);
                _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Default;
                SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
                SetBodyTracking(sampleInputManager);
                OvrAvatarLipSyncContext lipSyncInput = FindObjectOfType<OvrAvatarLipSyncContext>();
                SetLipSync(lipSyncInput);
                gameObject.name = "LocalAvatar";
            }
            else
            {
                SetIsLocal(false);
                _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
                gameObject.name = "RemoteAvatar";
            }
        }

        IEnumerator TryToLoadUser()
        {
            //We wait until the oculusID is set and the app token has been set
            while (!userIDSet || !OvrAvatarEntitlement.AccessTokenIsValid)
            {
                yield return null;
            }
            var hasAvatarRequest = OvrAvatarManager.Instance.UserHasAvatarAsync(_userId);
            while (hasAvatarRequest.IsCompleted == false)
            {
                yield return null;
            }
            LoadUser();
        }

        //Callback to know when the skeleton was loaded
        protected override void OnSkeletonLoaded()
        {
            base.OnSkeletonLoaded();
            skeletonLoaded = true;
            EventManager.TriggerEvent("SKELETON_LOADED");
        }

        //If the skeleton is already loaded, we can start streaming the avatar state every "_intervalToSendData" seconds
        private void LateUpdate()
        {
            if (!skeletonLoaded) return;
            float elapsedTime = Time.time - _cycleStartTime;
            if (elapsedTime > _intervalToSendData)
            {
                RecordAndSendStreamDataIfHasAuthority();
                _cycleStartTime = Time.time;
            }
        }

        //We "record" our avatar state and send it to other users, only if avatar is local (is ours)
        void RecordAndSendStreamDataIfHasAuthority()
        {
            if (IsLocal)
            {
                byte[] bytes = RecordStreamData(streamLOD);
                _photonView.RPC("RPC_RecieveStreamData", RpcTarget.Others, bytes);
            }
        }

        [PunRPC]
        public void RPC_RecieveStreamData(byte[] bytes)
        {
            AddToStreamDataList(bytes);
        }

        [PunRPC]
        public void RPC_SetOculusID(long id)
        {
            _userId = (ulong)id;
            userIDSet = true;
        }

        //Receive the recorded state of the avatar and add it to the List
        internal void AddToStreamDataList(byte[] bytes)
        {
            if (_streamedDataArray.Count == _maxBytesToLog)
            {
                _streamedDataArray.RemoveAt(_streamedDataArray.Count - 1);
            }
            _streamedDataArray.Add(bytes);
        }

        //If avatar is not local, and the _streamedDataArray has any values in it, we take the first element and apply it to the avatar
        private void Update()
        {
            if (!skeletonLoaded) return;
            if (_streamedDataArray.Count > 0)
            {
                if (!IsLocal)
                {
                    byte[] firstBytesInList = _streamedDataArray[0];
                    if (firstBytesInList != null)
                    {
                        //Apply the remote avatar state and smooth the animation
                        ApplyStreamData(firstBytesInList);
                        SetPlaybackTimeDelay(_intervalToSendData / 2);
                    }
                    _streamedDataArray.RemoveAt(0);
                }
            }
        }
    }
}