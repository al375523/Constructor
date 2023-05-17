using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ConstructionType
{
    Others,
    Structure,
    Equipment,
    Ceiling,
    Plants,
    People,
    Decoration
}
public class ConstructionItem : MonoBehaviour
{
    bool isVisible = true;
    public ConstructionType type = ConstructionType.Others;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    internal void ShowHideGo()
    {
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);             
    }

    public void SetChildren(GameObject go)
    {
        go.transform.parent = this.transform;
    }
}
