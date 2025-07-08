using UnityEngine;
using TilesManager;
using FungiSystem;

public class TerrainManager : MonoBehaviour
{
    #region Variables 
    [Header("Grid setup")]
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public float sizePrefab;

    [Header("Prefabs")]
    [SerializeField] public GameObject tilePrefab;
    [SerializeField] public GameObject treePrefab;
    #endregion

    #region Private variables
    public TilesSystem tileSystem;

    private string mushroomId = "Boletus Loyo", mushroomId2 = "Mycena Cyanocephala";
    private MushroomInstance activeMushroom, activeMushroom2;

    public float tileHeight;
    #endregion

    #region Start
    void Start()
    {
        tileSystem = new TilesSystem(width, height, sizePrefab, tilePrefab, treePrefab);
        tileSystem.GenerateHexGrid();

        Tile tile = tileSystem.grid[0, 0];

        if (MushroomFactory.CanPlaceMushroomOnTile(mushroomId, tile.tileType.ToString()))
        {
            activeMushroom = MushroomManager.CreateMushroom(mushroomId, tile.worldPosition, tile);
            tile.mushroom = activeMushroom;
            tile.isOccupied = true;
        }
        else
        {
            Debug.Log($"No se puede colocar {mushroomId} en un tile de tipo {tile.tileType}");
        }

        Tile tile2 = tileSystem.grid[1,1];
        tile2.tileType = TileType.DeadTree;

        if (MushroomFactory.CanPlaceMushroomOnTile(mushroomId2, tile2.tileType.ToString()))
        {
            activeMushroom2 = MushroomManager.CreateMushroom(mushroomId2, tile2.worldPosition, tile2);
            tile2.mushroom = activeMushroom2;
            tile2.isOccupied = true;
        }
        else
        {
            Debug.Log($"No se puede colocar {mushroomId2} en un tile de tipo {tile.tileType}");
        }


    }
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (activeMushroom != null)
            {
                activeMushroom.AdvanceStage(sizePrefab);
                activeMushroom2.AdvanceStage(sizePrefab);

                Debug.Log("Avanzó de etapa.");
            }
        }
    }
    #endregion
}