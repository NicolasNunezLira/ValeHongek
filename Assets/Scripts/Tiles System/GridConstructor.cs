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

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    float xPos = x * offsetX;
                    float zPos = z * offsetZ + (x % 2 == 1 ? offsetZ / 2f : 0f);

                    Vector3 worldPos = new Vector3(xPos, 0.01f, zPos);

                    GameObject tileGO = UnityEngine.Object.Instantiate(tilePrefab, worldPos, Quaternion.identity);
                    tileGO.name = $"Tile_{x}_{z}";

                    // Asignar referencia del borde si es necesario
                    TileHighlighter highlighter = tileGO.GetComponent<TileHighlighter>();
                    if (highlighter != null)
                    {
                        highlighter.borderObject = tileGO.transform.Find("Borde")?.gameObject;
                    }

                    tileGO.SetActive(true);

                    Tile tile = new Tile(worldPos, TileType.Understory, tileGO, 1, FungiSystem.SubstrateType.Tierra, "Ninguno");
                    grid[x, z] = tile;
                }
            }
        }


    }
}