using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class CircuitItems
{
    public List<WaterTankItem> WaterTanks = new List<WaterTankItem>();
    public List<BoilerItem> Boilers = new List<BoilerItem>();
    public List<ElbowItem> Elbows = new List<ElbowItem>();
    public List<PipeItem> Pipes = new List<PipeItem>();
    public List<ReductorItem> Reductors = new List<ReductorItem>();
    public List<ValveItem> Valves = new List<ValveItem>();

    public string date;
    public string fileName;
    public int idGreenhouse=0;
    public string sector;

    public CircuitItems(List<WaterTankItem> waterTanks, List<BoilerItem> boilers, List<ElbowItem> elbows, List<PipeItem> pipes, List<ReductorItem> reductors, List<ValveItem> valves,
        string date,  string fileName, int idGreenhouse, string sector) 
    {
        this.date = date;
        this.fileName = fileName;
        this.idGreenhouse = idGreenhouse;
        this.sector = sector;
        Elbows = elbows;
        Pipes = pipes;
        Reductors = reductors;
        Valves = valves;
        WaterTanks = waterTanks;
        Boilers = boilers;
    }

    public GameObject InstantiatePrefabWithID(string id, Dictionary<string, string> IDToPrefabName)
    {
        if (!IDToPrefabName.ContainsKey(id)) Debug.LogWarning("There is no prefab with that ID");

        return LoadPrefab("Circuit/", id);
    }

    public GameObject LoadPrefab(string folder, string prefSearch)
    {
        return Resources.Load<GameObject>("Prefabs/" + folder + prefSearch);
    }

    public void ClearCircuit()
    {
        WaterTanks.Clear();
        Boilers.Clear();
        Elbows.Clear();
        Pipes.Clear();
        Reductors.Clear();
        Valves.Clear();
    }
}