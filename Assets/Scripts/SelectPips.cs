using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPips : MonoBehaviour
{
    MeshCollider meshCollider;
    public void HandsNear()
    {
        transform.parent.GetComponent<CircuitItem>().SelectItemHands();
    }

    public void SetColliderSize()
    {
        meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        meshCollider.sharedMesh = transform.parent.GetChild(0).GetComponent<MeshCollider>().sharedMesh;
    }
}
