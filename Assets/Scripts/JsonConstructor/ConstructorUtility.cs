using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConstructorUtility
{
    [HideInInspector]
    public string wordToSearch;
    [HideInInspector]
    public List<GameObject> gosFound= new List<GameObject>();

    public List<GameObject> SearchGOsThatStartWith(string s)
    {
        return GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.StartsWith(s)).Distinct().ToList();
    }

    public List<GameObject> SearchGOsRegex(string s)
    {
        return GameObject.FindObjectsOfType<GameObject>().Where(obj => Regex.IsMatch(obj.name, s, RegexOptions.IgnoreCase)).Distinct().ToList();

    }

    public List<string> GetNamesSearched(List<GameObject> gos)
    {
        return gos.Select(obj => obj.name).ToList();
    }

    public UnityEngine.Object GetGo(List<GameObject> gos, string name)
    {
        return (UnityEngine.Object)gos.Where(obj => obj.name == name).First();
    }
}
