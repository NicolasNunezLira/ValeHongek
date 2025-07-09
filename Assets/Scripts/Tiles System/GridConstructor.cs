using FungiSystem;
using UnityEngine;
using System.Collections.Generic;

namespace TilesManager
{
    public partial class TilesSystem
    {
        private GameObject tileParent, treeParent;

        public List<TreeGroup> allTreeGroups = new();
        public List<BigTreeGroup> allBigTreeGroups = new();

        #region Grid Generator
        public void GenerateHexGrid()
        {
            grid = new Tile[width, height];

            MeshFilter mf = tilePrefab.GetComponentInChildren<MeshFilter>();
            Bounds bounds = mf.sharedMesh.bounds;
            Vector3 scale = tilePrefab.transform.localScale;
            float hexWidth = bounds.size.x * scale.x;
            float hexHeight = bounds.size.z * scale.z;

            float separationFactorX = 1.0622f;
            float separationFactorZ = 1.05f;
            float offsetX = hexWidth * 0.75f * separationFactorX;
            float offsetZ = hexHeight * separationFactorZ;

            tileParent = new GameObject("Tiles");
            treeParent = new GameObject("Trees");

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    if (grid[x, z] != null)
                        continue;

                    if (Random.value < 0.1f && IsValidBigTreeGroupCenter(x, z))
                    {
                        CreateBigTreeGroupAt(x, z);
                        continue;
                    }

                    if (Random.value < 0.25f)
                    {
                        CreateTreeGroup(x, z);
                        continue;
                    }

                    CreateTileAt(x, z, TileType.Understory);
                }
            }
        }
        #endregion

        #region Tile Creation
        private Tile CreateTileAt(int x, int z, TileType type)
        {
            if (grid[x, z] != null)
            {
                if (!grid[x, z].isOccupied)
                    grid[x, z].tileType = type;
                return grid[x, z];
            }

            MeshFilter mf = tilePrefab.GetComponentInChildren<MeshFilter>();
            Bounds bounds = mf.sharedMesh.bounds;
            Vector3 scale = tilePrefab.transform.localScale;
            float hexWidth = bounds.size.x * scale.x;
            float hexHeight = bounds.size.z * scale.z;

            float separationFactorX = 1.0622f;
            float separationFactorZ = 1.05f;
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
                highlighter.tile = tile;

            return tile;
        }
        #endregion

        #region Tree Creation
        public void CreateTreeGroup(int x, int z)
        {
            int z2 = (x % 2 == 0) ? z - 1 : z + 1;
            if (x + 1 >= width || z2 < 0 || z2 >= height)
                return;

            Tile t0 = CreateTileAt(x, z, TileType.Tree);
            Tile t1 = CreateTileAt(x + 1, z, TileType.Tree);
            Tile t2 = CreateTileAt(x + 1, z2, TileType.Tree);

            if (t0.isOccupied || t1.isOccupied || t2.isOccupied)
                return;

            TreeGroup group = new TreeGroup(t0, t1, t2);
            allTreeGroups.Add(group);

            Vector3 sharedVertexPos = (t0.worldPosition + t1.worldPosition + t2.worldPosition) / 3f;
            sharedVertexPos.y += 0.01f;

            GameObject tree = Object.Instantiate(treePrefab, sharedVertexPos, Quaternion.identity, treeParent.transform);
            tree.name = $"Tree_{x}_{z}";
            tree.SetActive(true);
            group.treeInstance = tree;

            t0.isOccupied = t1.isOccupied = t2.isOccupied = true;
        }
        #endregion

        #region Big Tree Creation
        private bool IsValidBigTreeGroupCenter(int x, int z)
        {
            int[,] offsets = (x % 2 == 0) ? new int[,]
            {
                { 0, 0 }, { -1, 0 }, { +1, 0 },
                { 0, -1 }, { 0, +1 }, { -1, -1 }, { +1, -1 }
            } : new int[,]
            {
                { 0, 0 }, { -1, 0 }, { +1, 0 },
                { 0, -1 }, { 0, +1 }, { -1, +1 }, { +1, +1 }
            };

            for (int i = 0; i < 7; i++)
            {
                int nx = x + offsets[i, 0];
                int nz = z + offsets[i, 1];

                if (nx < 0 || nx >= width || nz < 0 || nz >= height)
                    return false;

                Tile t = grid[nx, nz];
                if (t != null && t.isOccupied)
                    return false;
            }

            return true;
        }

        private void CreateBigTreeGroupAt(int x, int z)
        {
            int[,] offsets = (x % 2 == 0) ? new int[,]
            {
                { 0, 0 }, { -1, 0 }, { +1, 0 },
                { 0, -1 }, { 0, +1 }, { -1, -1 }, { +1, -1 }
            } : new int[,]
            {
                { 0, 0 }, { -1, 0 }, { +1, 0 },
                { 0, -1 }, { 0, +1 }, { -1, +1 }, { +1, +1 }
            };

            List<Tile> tiles = new();

            for (int i = 0; i < 7; i++)
            {
                int nx = x + offsets[i, 0];
                int nz = z + offsets[i, 1];

                Tile t = CreateTileAt(nx, nz, TileType.Tree);
                if (t.isOccupied) return; // abortar si alguno está ocupado

                tiles.Add(t);
            }

            BigTreeGroup group = new BigTreeGroup(
                tiles[0], tiles[1], tiles[2], tiles[3], tiles[4], tiles[5], tiles[6]
            );
            allBigTreeGroups.Add(group);

            Vector3 centerPos = tiles[0].worldPosition + Vector3.up * 0.01f;
            GameObject bigTree = Object.Instantiate(bigTreePrefab, centerPos, Quaternion.identity, treeParent.transform);
            bigTree.name = $"BigTree_{x}_{z}";
            bigTree.SetActive(true);
            group.treeInstance = bigTree;

            foreach (var tile in tiles)
                tile.isOccupied = true;
        }
        #endregion
    }
}
