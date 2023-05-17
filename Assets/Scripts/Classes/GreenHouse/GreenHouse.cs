
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GreenHouse: IInstantiate
{
    public CropGutters[] CropGutters;
    public CoordinatesVerticalGrid[] CoordinatesVerticalGrid;
    public Walls[] Walls;
    public Lamps[] Lamps;
    public Fans[] Fans;
    public Doors[] Doors;
    public Tanks[] Tanks;

    private JsonReader _jsonReader;
    public GreenHouse(CropGutters[] cropGutters, CoordinatesVerticalGrid[] coordinatesVerticalGrid, Walls[] walls, Lamps[] lamps, Fans[] fans, Doors[] doors, Tanks[] tanks)
    {
        CropGutters = cropGutters;
        CoordinatesVerticalGrid = coordinatesVerticalGrid;
        Walls = walls;
        Lamps = lamps;
        Fans = fans;
        Doors = doors;
        Tanks = tanks;
    }


    public GameObject InstantiatePrefabWithID(string id, Dictionary<string, string> IDToPrefabName)
    {
        if (!IDToPrefabName.ContainsKey(id)) Debug.LogWarning("There is no prefab with that ID");

        return LoadPrefab("GreenHouse/", IDToPrefabName[id]);
    }
    
    public GameObject LoadPrefab(string folder, string prefSearch)
    {
        return Resources.Load<GameObject>("Prefabs/" + folder + prefSearch);
    }

  public void InstantiateItems(string idData, params GreenHouseItem[] itemsToInstantiate)
    {
        _jsonReader = Object.FindObjectOfType<JsonReader>();
        if (_jsonReader == null) Debug.LogWarning("Json Reader is null");
        GameObject prefab = InstantiatePrefabWithID(idData, _jsonReader.IDToPrefabName);
        GameObject itemInstantiated = null;
        foreach (var item in itemsToInstantiate)
        {
            itemInstantiated  = Object.Instantiate(prefab, item.pivotPosition, item.Rotation.ToQuaternion());
        }
        
        _jsonReader.SearchAndAssignParent(idData, itemInstantiated);
        
    }
}