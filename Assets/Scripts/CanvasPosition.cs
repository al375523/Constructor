using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPosition : MonoBehaviour
{
    GameObject canvasPos;
    GameObject cameraObj;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasPos = GameObject.FindGameObjectWithTag("Ubication");
        cameraObj = Camera.main.gameObject;
        transform.position = canvasPos.transform.position;
        transform.LookAt(cameraObj.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReubicateUI()
    {        
        transform.position = new Vector3(canvasPos.transform.position.x, canvasPos.transform.position.y, canvasPos.transform.position.z);        
    }
}
