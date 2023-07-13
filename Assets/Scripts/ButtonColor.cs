using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    public Color color;
    public Image image;

    Color originalColor;
    // Start is called before the first frame update
    void Start()
    {       
        if(image!=null)
            originalColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Marker")
        {
            image.color = color;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Marker")
        {
            image.color = originalColor;
        }
    }


}
