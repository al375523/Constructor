    using System.Collections.Generic;
    using UnityEngine;

    public interface IInstantiate
    {
       
        public GameObject InstantiatePrefabWithID(string id, Dictionary<string, string> IDToPrefabName);
        public GameObject LoadPrefab(string folder, string prefSearch);
  

    }
