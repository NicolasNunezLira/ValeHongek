using FungiSystem;
using UnityEngine;

namespace TilesManager
{
    
    public partial class TilesSystem
    {
        public GameObject tileParent;
        public void GenerateHexGrid()
        {
            grid = new Tile[width, height];

            // Mide el tamaño real del tilePrefab en Unity
            MeshFilter mf = tilePrefab.GetComponentInChildren<MeshFilter>();
            Bounds bounds = mf.sharedMesh.bounds;

            // Escala de prefab
            Vector3 scale = tilePrefab.transform.localScale;
            float hexWidth = bounds.size.x * scale.x;
            float hexHeight = bounds.size.z * scale.z;

            float separationFactorX = 1.0622f; // Aumenta si aún se solapan (ajusta entre 1.02 - 1.1)
            float separationFactorZ = 1.05f; // Aumenta si aún se solapan (ajusta entre 1.02 - 1.1)
            float offsetX = hexWidth * 0.75f * separationFactorX;
            float offsetZ = hexHeight * separationFactorZ;

            tileParent = new GameObject("Tiles");

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    float xPos = x * offsetX;
                    float zPos = z * offsetZ + (x % 2 == 1 ? offsetZ / 2f : 0f);

                    Vector3 worldPos = new Vector3(xPos, 0.01f, zPos);

                    GameObject tileGO = UnityEngine.Object.Instantiate(tilePrefab, worldPos, Quaternion.identity, tileParent.transform);
                    tileGO.name = $"Tile_{x}_{z}";

                    tileGO.SetActive(true);

                    Tile tile = new Tile(worldPos, TileType.Understory, null, 1, FungiSystem.SubstrateType.Tierra, "Ninguno");
                    grid[x, z] = tile;
                    
                    TileHighlighter tileHighlighter = tileGO.GetComponentInChildren<TileHighlighter>();
                    if (tileHighlighter != null)
                    {
                        tileHighlighter.tile = tile; // Ajusta la altura del resaltado
                    }
                }
            }
        }


    }
}