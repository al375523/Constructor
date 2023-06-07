using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructionPanel : MonoBehaviour
{
    public ConstructionItem[] constructionItems;
    public bool[] auxItems;
    ProjectManagerDemo projectManagerDemo;
    string actualSection;
  
    private void Awake()
    {
        projectManagerDemo = FindObjectOfType<ProjectManagerDemo>();
        EventManager.StartListening("LOADING_SECTION", LoadingSection);
    }

    private void OnEnable()
    {
        if (constructionItems==null || constructionItems.Length==0)
            constructionItems = FindObjectsOfType(typeof(ConstructionItem)) as ConstructionItem[];
    }

    public void ShowHideAllItemsOfType(ConstructionType curType)
    {
        foreach (var item in constructionItems)
        {
            if (item.type== curType)
            {
                item.ShowHideGo();
            }
        }
    }

    public void LoadingSection()
    {

        auxItems = new bool[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                auxItems[i] = true;
            else auxItems[i] = false;
        }
            

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);

        constructionItems = FindObjectsOfType(typeof(ConstructionItem)) as ConstructionItem[];
        for (int i = 0; i < auxItems.Length; i++)
            if (!auxItems[i])
                transform.GetChild(i).gameObject.SetActive(false);
    }   
    
    public void ShowAllItems()
    {
        foreach (var item in constructionItems)
            item.gameObject.SetActive(true);
    }
}