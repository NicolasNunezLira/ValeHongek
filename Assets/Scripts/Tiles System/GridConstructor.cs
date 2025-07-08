using FungiSystem;
using UnityEngine;

namespace TilesManager
{
    public partial class TilesSystem
    {
        private GameObject tileParent, treeParent;

        #region Grid Generator
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

            treeParent = new GameObject("Trees");

            
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    // Saltar si ya hay tile
                    if (grid[x, z] != null)
                        continue;

                    // Intentar formar grupo tipo Tree
                    if (Random.value < 0.1f)
                    {
                        int z2 = (x % 2 == 0) ? z - 1 : z + 1;

                        if (x + 1 < width && z2 >= 0 && z2 < height &&
                            grid[x, z] == null &&
                            grid[x + 1, z] == null &&
                            grid[x + 1, z2] == null)
                        {
                            Tile t0 = CreateTileAt(x, z, TileType.Tree);
                            Tile t1 = CreateTileAt(x + 1, z, TileType.Tree);
                            Tile t2 = CreateTileAt(x + 1, z2, TileType.Tree);

                            TreeGroup group = new TreeGroup(t0, t1, t2);

                            // Instanciar treePrefab en el vértice compartido
                            Vector3 p0 = t0.worldPosition;
                            Vector3 p1 = t1.worldPosition;
                            Vector3 p2 = t2.worldPosition;

                            // Calcular centro del triángulo para aproximar el vértice compartido
                            Vector3 sharedVertexPos = (p0 + p1 + p2) / 3f;
                            sharedVertexPos.y += 0.01f; // levanta levemente para evitar z-fighting

                            GameObject tree = Object.Instantiate(treePrefab, sharedVertexPos, Quaternion.identity, treeParent.transform);
                            tree.name = $"Tree_{x}_{z}";
                            tree.SetActive(true);

                            group.treeInstance = tree;

                            continue;
                        }
                    }


                    // Si no es grupo Tree, crear tile normal
                    Tile tile = CreateTileAt(x, z, TileType.Understory);

                }
            }

        }
        #endregion

        #region Tile Creation
        private Tile CreateTileAt(int x, int z, TileType type)
        {

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

            float xPos = x * offsetX;
            float zPos = z * offsetZ + (x % 2 == 1 ? offsetZ / 2f : 0f);

            Vector3 worldPos = new Vector3(xPos, 0.01f, zPos);
            GameObject tileGO = Object.Instantiate(tilePrefab, worldPos, Quaternion.identity, tileParent.transform);
            tileGO.name = $"Tile_{x}_{z}";
            tileGO.SetActive(true);

            Tile tile = new Tile(worldPos, type, tileGO, 1, SubstrateType.Tierra, "Ninguno");
            grid[x, z] = tile;

            TileHighlighter highlighter = tileGO.GetComponentInChildren<TileHighlighter>();
            if (highlighter != null)
            {
                highlighter.tile = tile;
            }

            return tile;
        }
        #endregion
    }

}