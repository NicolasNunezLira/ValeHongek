using FungiSystem;
using UnityEngine;

namespace TilesManager
{
    public partial class TilesSystem
    {
        public void GenerateGrid()
        {
            grid = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 worldPos = new Vector3(x * tileSize, 0.01f, z * tileSize);
                    tileGO = UnityEngine.Object.Instantiate(tilePrefab, worldPos, Quaternion.identity);
                    tileGO.name = $"Tile_{x}_{z}";
                    MeshFilter mf = tileGO.GetComponentInChildren<MeshFilter>();
                    if (mf != null && mf.sharedMesh != null)
                    {
                        Vector3 size = mf.sharedMesh.bounds.size;
                        float scaleX = tileSize / size.x;
                        float scaleZ = tileSize / size.z;

                        tileGO.transform.localScale = new Vector3(scaleX, 1, scaleZ);
                    }
                    tileGO.SetActive(true);

                    

                    // Crear y guardar el Tile
                    Tile tile = new Tile(worldPos, TileType.Understory, tileGO, 1, FungiSystem.SubstrateType.Tierra, "Ninguno");
                    grid[x, z] = tile;
                }
            }
        }
    }
}