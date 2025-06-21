using UnityEngine;
using TilesManager;
using System.Collections.Generic;

namespace FungiSystem
{
    [System.Serializable]
    public partial class MushroomSystem
    {
        public Tile[,] grid;
        public int width;
        public int height;
        public MushroomSystem(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new Tile[width, height];
        }
    }
}