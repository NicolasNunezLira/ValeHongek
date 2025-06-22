using FungiSystem;
using UnityEngine;
using System.Collections.Generic;

namespace TilesManager
{
    [System.Serializable]
    public class TileTypePrefab
    {
        public TileType type;
        public GameObject prefab;
    }

    public class TileVisualLibrary : MonoBehaviour
    {
        public List<TileTypePrefab> tilePrefabs;
        private Dictionary<TileType, GameObject> prefabMap;

        void Awake()
        {
            prefabMap = new Dictionary<TileType, GameObject>();

            foreach (var entry in tilePrefabs)
            {
                prefabMap[entry.type] = entry.prefab;
            }
        }

        public GameObject GetPrefab(TileType type)
        {
            return prefabMap.TryGetValue(type, out var go) ? go : null;
        }
    }
}