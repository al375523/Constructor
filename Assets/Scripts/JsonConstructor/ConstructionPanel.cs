using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructionPanel : MonoBehaviour
{
    public ConstructionItem[] constructionItems;
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

    void LoadingSection()
    {
        constructionItems = FindObjectsOfType(typeof(ConstructionItem)) as ConstructionItem[];
    }    
}
