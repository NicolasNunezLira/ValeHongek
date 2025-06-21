using System;
using System.Collections.Generic;
using UnityEngine;

namespace FungiSystem
{

    [Serializable]
    public class PrefabEntry
    {
        public string key;
        public string value;
    }

    public class TimeEntry
    {
        public string key;
        public int value;
    }


    [Serializable]
    public class MushroomData
    {
        public string commonName;
        public string scientificName;
        public List<PrefabEntry> prefabList;
        public List<string> validTilesTypes;
        public List<string> substrate;
        public List<string> relation;
        public List<string> asociatedTrees;
        public List<TimeEntry> timeList;

        [NonSerialized]
        public Dictionary<string, string> prefabs;

        [NonSerialized]
        public Dictionary<string, int> times;

        public void BuildPrefabMap()
        {
            prefabs = new Dictionary<string, string>();
            foreach (var entry in prefabList)
            {
                prefabs[entry.key] = entry.value;
            }

            if (timeList != null)
            {
                times = new Dictionary<string, int>();
                foreach (var entry in timeList)
                {
                    times[entry.key] = entry.value;
                }
            }
        }
    }

    [Serializable]
    public class MushroomDataList
    {
        public List<MushroomData> mushrooms;
    }
}