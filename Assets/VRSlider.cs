using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class VRSlider : MonoBehaviour
{
    public GameObject elipse;
    public Vector3 hitPoint;
    public TMP_Text valueText;
    public string sliderType;
    public bool isSubmenu = false;
    [HideInInspector] public ProjectManagerDemo projectManager;

    BoxCollider boxCollider;
    float value = 10f;
    Transform playerTransform;
    float originalY;
    GameObject submenu = null;
    GameObject ubication;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        valueText.text = value.ToString();
        if(sliderType == "Height")
        {
            valueText.text = "0.1";
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            projectManager = GameObject.FindGameObjectWithTag("ProjectManager").GetComponent<ProjectManagerDemo>();
            if (isSubmenu)
            {
                submenu = GameObject.FindGameObjectWithTag("Submenu");
                ubication = GameObject.FindGameObjectWithTag("Ubication");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    public void OnRaycastReceived()
    {        
        elipse.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
        CheckCase();
    }

    void CheckCase()
    {
        float size = boxCollider.bounds.max.y - boxCollider.bounds.min.y;
        float hitInSlider = elipse.transform.position.y - boxCollider.bounds.min.y;      
        switch (sliderType)
        {
            default:
                break;
            case "Height":
                value = hitInSlider / size;
                originalY = projectManager.originalY;
                playerTransform.position = new Vector3(playerTransform.position.x, originalY + value, playerTransform.position.z);
                projectManager.actualY = originalY + value;
                value = Mathf.Round(value * 10f) / 10f;
                if (submenu != null) UpdatePosition();                
                break;
            case "Audio":
                value = Mathf.RoundToInt(hitInSlider / size * 100);
                AudioListener.volume = value;              
                break;
        }
        valueText.text = value.ToString();
    }

    public void ResetValue()
    {
        if (sliderType == "Height")
        {
            playerTransform.position = new Vector3(playerTransform.position.x, originalY, playerTransform.position.z);
            valueText.text = value.ToString();
            if (submenu != null) UpdatePosition();
        }
        else
        {
            AudioListener.volume = 10f;
            valueText.text = AudioListener.volume.ToString();
        }
    }

    void UpdatePosition()
    {
        submenu.transform.position = new Vector3(submenu.transform.position.x, ubication.transform.position.y, submenu.transform.position.z);
    }
}
