using System.Collections.Generic;
using TilesManager;
using UnityEngine;

namespace FungiSystem
{
    public static class MushroomManager
    {
        private static List<MushroomInstance> mushrooms = new List<MushroomInstance>();

        public static MushroomInstance CreateMushroom(string scientificName, Vector3 position, Tile tile)
        {
            var data = MushroomFactory.GetData(scientificName);
            if (data == null)
            {
                Debug.LogError($"Mushroom data for {scientificName} not found.");
                return null;
            }

            // Usa directamente el prefab inicial
            string initialStage = "hyphalKnot";
            GameObject prefab = Resources.Load<GameObject>(data.prefabs[initialStage]);
            if (prefab == null)
            {
                Debug.LogError($"Prefab for stage '{initialStage}' not found.");
                return null;
            }

            GameObject instanceGO = GameObject.Instantiate(prefab, position, Quaternion.identity);
            instanceGO.name = $"{data.commonName}_{initialStage}";

            var instance = new MushroomInstance(scientificName, initialStage, instanceGO, tile);
            mushrooms.Add(instance);
            return instance;
        }

        public static void AdvanceAll()
        {
            foreach (var m in mushrooms)
            {
                //m.AdvanceStage();
            }
        }

        public static void ClearAll()
        {
            foreach (var m in mushrooms)
            {
                if (m.gameObject != null)
                    GameObject.Destroy(m.gameObject);
            }
            mushrooms.Clear();
        }

        public static List<MushroomInstance> GetAll()
        {
            return mushrooms;
        }
    }
}
