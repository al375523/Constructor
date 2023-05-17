using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class SearchAndReplace: ConstructorUtility
{

    public GameObject goToReplace;
    // Start is called before the first frame update

    public void ReplaceCurrentSearch(bool samePosition, bool sameRotation, bool sameScale)
    {
        foreach (var go in gosFound)
        {
            var parent = go.transform.parent;
            var newGo = PrefabUtility.InstantiatePrefab(goToReplace, parent) as GameObject;
            if (samePosition) newGo.transform.position = go.transform.position;
            if (sameRotation) newGo.transform.rotation = go.transform.rotation;
            if (sameScale) newGo.transform.rotation = go.transform.rotation;
            GameObject.DestroyImmediate(go);
        }
        gosFound.Clear();
        /*   for (int i = gosFound.Count-1; i >0; i--)
           {
               GameObject.Destroy(gosFound[i]);
           }
           */
    }

    public void Clean() {
         wordToSearch="";
         goToReplace=null;
         gosFound = new List<GameObject>();
    }
}
