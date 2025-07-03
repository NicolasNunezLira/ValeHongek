using FungiSystem;
using UnityEngine;

namespace TilesManager
{
    [System.Serializable]
    public class Tile
    {
        public bool isOccupied;
        public Vector3 worldPosition;
        public TileType tileType;
        public GameObject go;
        public float humedad;
        public SubstrateType sustrato;
        public string arbol;

        public MushroomInstance mushroom;

        public Tile(Vector3 position, TileType type, GameObject tileInstance,
            float humedad, SubstrateType sustrato, string arbol)
        {
            worldPosition = position;
            tileType = type;
            go = tileInstance;
            isOccupied = false;
            this.humedad = humedad;
            this.sustrato = sustrato;
            this.arbol = arbol;
        }

        // Puedes agregar más lógica aquí, como:
        public bool IsBuildable()
        {
            return !isOccupied && tileType != TileType.Water && tileType != TileType.Road;
        }
    }

    [System.Serializable]
    public enum TileType
    {
        Understory,
        Water,
        Road,
        Swamp,
        Tree
    }
}