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

            //MushroomData[] mushroomArray = JsonHelper.FromJson<MushroomData>(jsonText.text);
            MushroomDataList list = JsonUtility.FromJson<MushroomDataList>(jsonText.text);
            foreach (var data in list.mushrooms)
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

        public static GameObject CreateMushroom(string scientificName, Vector3 position, float tileSize)
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

            Renderer rend = instance.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                Vector3 size = rend.bounds.size;
                float maxDim = Mathf.Max(size.x, size.z);

                // Escala proporcional para que quepa en el tile
                if (maxDim > 0)
                {
                    float scaleFactor = tileSize / maxDim;
                    instance.transform.localScale = instance.transform.localScale * scaleFactor;
                }

                // Ajustar posición vertical para que el hongo quede apoyado sobre el tile
                float height = rend.bounds.size.y;
                instance.transform.position = new Vector3(position.x, position.y + height / 2f, position.z);
            }
            else
            {
                // Si no tiene Renderer, posición normal
                instance.transform.position = position;
            }

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