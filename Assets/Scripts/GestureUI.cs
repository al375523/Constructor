using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureUI : MonoBehaviour
{
    public GameObject canvas;
    GestureDetector[] gestures;
    bool alreadyOpen = false;
    bool cooldown = false;
    public Transform leftHand;
    public Transform rightHand;
    // Start is called before the first frame update
    void Start()
    {
        gestures = GameObject.Find("GestureDetection").GetComponents<GestureDetector>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (!cooldown && gestures[1].actualGesture.name == "Close")
        {
            if (alreadyOpen)
            {
                alreadyOpen = false;
                canvas.SetActive(false);
            }
            else if (!alreadyOpen)
            {
                canvas.SetActive(true);
                alreadyOpen = true;                
            }
            cooldown = true;
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1f);
        cooldown = false;
    }

    public void Test()
    {
        Debug.Log("Funciona");
    }
}
