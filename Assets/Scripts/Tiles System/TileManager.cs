using UnityEngine;

namespace TilesManager
{
    [System.Serializable]
    public partial class TilesSystem
    {
        #region Variables
        public int width;
        public int height;
        public float tileSize;
        public Tile[,] grid;
        public GameObject tilePrefab, tileGO, treePrefab;
        #endregion

        #region Init Tiles System
        public TilesSystem(
            int width,
            int height,
            float tileSize,
            GameObject tilePrefab,
            GameObject treePrefab
        )
        {
            this.width = width;
            this.height = height;
            this.tileSize = tileSize;
            this.tilePrefab = tilePrefab;
            this.treePrefab = treePrefab;
        }
        #endregion        
    }
}