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

    [Header("Tile's prefab")]
    [SerializeField] public GameObject tilePrefab;
    #endregion

    #region Private variables
    private GameObject tileGO;
    public TilesSystem tileSystem;

    private string mushroomId = "Boletus Loyo", mushroomId2 = "Mycena Cyanocephala";
    private MushroomInstance activeMushroom, activeMushroom2;

    public float tileHeight;
    #endregion

    #region Start
    void Start()
    {
        tileGO = Instantiate(tilePrefab);
        tileHeight = tileGO.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.y * tileGO.transform.localScale.y;
        tileGO.SetActive(false);

        tileSystem = new TilesSystem(width, height, sizePrefab, tilePrefab);
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