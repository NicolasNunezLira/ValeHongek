using System.Collections.Generic;
using UnityEngine;

namespace FungiSystem
{
    public static class MushroomFactory
    {
        private static Dictionary<string, MushroomData> mushroomMap = new Dictionary<string, MushroomData>();
        private static bool isInitialized = false;

        public static void InitFactory()
        {
            if (isInitialized) return;

            TextAsset jsonText = Resources.Load<TextAsset>("Mushrooms");
            if (jsonText == null)
            {
                Debug.LogError("Mushrooms.json not found in Resources.");
                return;
            }

            MushroomData[] mushroomArray = JsonHelper.FromJson<MushroomData>(jsonText.text);
            foreach (var data in mushroomArray)
            {
                data.BuildPrefabMap();
                mushroomMap[data.scientificName] = data;
            }

            isInitialized = true;
        }

        public static bool CanPlaceMushroomOnTile(string scientificName, string tileType)
        {
            if (!isInitialized) InitFactory();

            if (!mushroomMap.ContainsKey(scientificName)) return false;

            return mushroomMap[scientificName].validTilesTypes.Contains(tileType);
        }

        public static GameObject CreateMushroom(string scientificName, Vector3 position)
        {
            if (!isInitialized) InitFactory();

            if (!mushroomMap.ContainsKey(scientificName))
            {
                Debug.Log($"{scientificName} not found.");
                return null;
            }

            MushroomData data = mushroomMap[scientificName];
            GameObject prefab = Resources.Load<GameObject>(data.prefabs["hyphalKnot"]);

            if (prefab == null)
            {
                Debug.Log($"{data.prefabs["hyphalKnot"]} not found.");
                return null;
            }

            GameObject instance = GameObject.Instantiate(prefab, position, Quaternion.identity);
            instance.name = data.commonName;

            return instance;
        }

        public static MushroomData GetData(string scientificName)
        {
            if (!isInitialized) InitFactory();
            if (mushroomMap.TryGetValue(scientificName, out var data))
                return data;
            return null;
        }
    }
}