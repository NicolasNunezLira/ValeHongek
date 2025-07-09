using System.Collections.Generic;
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
        public TreeGroup treeGroup;
        public BigTreeGroup bigTreeGroup;

        public float height
        {
            get
            {
                if (go != null)
                {
                    MeshFilter mf = go.GetComponentInChildren<MeshFilter>();
                    if (mf != null)
                    {
                        return mf.sharedMesh.bounds.size.y * go.transform.localScale.y;
                    }
                }
                return 0f; // Valor por defecto si no se puede calcular la altura
            }
        }

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


        public bool IsBuildable()
        {
            return !isOccupied && tileType != TileType.Water && tileType != TileType.Road;
        }
    }

    public class TreeGroup
    {
        public List<Tile> tiles;
        public GameObject treeInstance;

        public TreeGroup(Tile t1, Tile t2, Tile t3)
        {
            tiles = new List<Tile> { t1, t2, t3 };
            foreach (Tile tile in tiles)
            {
                tile.treeGroup = this;
            }
        }
    }

    public class BigTreeGroup
    {
        public List<Tile> tiles;
        public GameObject treeInstance;

        public BigTreeGroup(
            Tile t1, Tile t2, Tile t3, Tile t4,
            Tile t5, Tile t6, Tile t7)
        {
            tiles = new List<Tile> { t1, t2, t3, t4, t5, t6, t7 };
            foreach (Tile tile in tiles)
            {
                tile.bigTreeGroup = this;
            }        
        }
    }

    [System.Serializable]
    public enum TileType
    {
        Understory,
        Water,
        Road,
        Swamp,
        Tree,
        DeadTree
    }
}