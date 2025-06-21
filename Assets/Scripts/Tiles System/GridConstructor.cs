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
                    Vector3 worldPos = new Vector3(x * tileSize, 0, z * tileSize);
                    tileGO = UnityEngine.Object.Instantiate(tilePrefab, worldPos, Quaternion.identity);
                    tileGO.name = $"Tile_{x}_{z}";
                    tileGO.transform.localScale = new Vector3(tileSize, 1, tileSize);
                    tileGO.SetActive(true);

                    // Puedes modificar el color según el tipo de tile si lo deseas
                    tileGO.GetComponent<Renderer>().material.color = Color.green;

                    // Crear y guardar el Tile
                    Tile tile = new Tile(worldPos, TileType.Understory, tileGO, 1, FungiSystem.SubstrateType.Tierra, "Ninguno");
                    grid[x, z] = tile;
                }
            }
        }
    }
}