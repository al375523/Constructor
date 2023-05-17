using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DeleteObjects : ConstructorUtility
{
    //public List<string> defaultGOToDelete = ["DK-10380", "DK-13390", "GA-01105", "GA-01143", "DK-08709", "DK-13526"]
    public string[] defaultGOToDelete = { "DK-10380", "DK-13390", "GA-01105", "GA-01143", "DK-08709", "DK-13526", "bolt", "screw", "^M10X.*", "^GA-010.*", "^GA-06.*", "^DK-09760.*", "^GA-01019.*", "^GA-01154.*", "^DK-14320.*", "^SK-02070.*", "^HVAC_Hanging-Systems.*", "^Collegamenti struttural.*", "^OK-08146.*", "^ANCHOR FOUNDATION PROFILE ANCHOR.*", "^M12X45.*" , "piastra" };

    // Start is called before the first frame update


    public string GetNameDefaultObjectsToDelete()
    {
        string res = "";
        foreach (var item in defaultGOToDelete)
        {
            res += item + " ,";
        }

        return res;
    }


    public List<string> GetNamesSearched()
    {
        return gosFound.Select(obj => obj.name).ToList();
    }



    public void DeleteFoundedObjects()
    {
        foreach (var go in gosFound)
        {
            GameObject.DestroyImmediate(go);
        }
        gosFound.Clear();

    }

    public void DeleteDefaultObjects()
    {
        foreach (var name in defaultGOToDelete)
        {
            var gos = SearchGOsRegex(name);
            foreach (var item in gos)
            {
                GameObject.DestroyImmediate(item);
            }
           
        }
        gosFound.Clear();
    }


    public void Clean()
    {
        wordToSearch = "";
        gosFound = new List<GameObject>();
    }

}
