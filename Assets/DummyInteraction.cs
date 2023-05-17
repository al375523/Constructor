using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInteraction : MonoBehaviour
{
    public GameObject idle;
    public GameObject ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerPunch")
        {
            idle.SetActive(false);
            ragdoll.SetActive(true);
        }
    }
}
