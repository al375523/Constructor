using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TrackingOculus : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    PhotonView photonView;

    Transform headRig; //MainCamera
    Transform leftHandRig; //Left Hand Controller
    Transform rightHandRig; //Right Hand Controller

    // Start is called before the first frame update
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            rightHand.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            head.gameObject.SetActive(false);

            MappingNodes(head, XRNode.Head);
            MappingNodes(rightHand, XRNode.RightHand);
            MappingNodes(leftHand, XRNode.LeftHand);
        }
    }

    void MappingNodes(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;

    }
}
