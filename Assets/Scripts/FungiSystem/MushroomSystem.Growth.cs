using UnityEngine;
using TilesManager;

namespace FungiSystem
{
    public partial class MushroomSystem
    {
        public void UpdateGrowth(float deltaTime)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tile = grid[x, y];
                    if (tile.mushroom != null)
                    {
                        //tile.mushroom.AdvanceStage(tileSize);
                    }
                }
            }
        }
    }
}