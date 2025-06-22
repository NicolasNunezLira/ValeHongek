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

    private string mushroomId = "Boletus Loyo";
    private MushroomInstance activeMushroom; 
    #endregion

    #region Start
    void Start()
    {
        tileGO = Instantiate(tilePrefab);
        tileGO.SetActive(false);

        tileSystem = new TilesSystem(width, height, sizePrefab, tilePrefab);
        tileSystem.GenerateGrid();

        Tile tile = tileSystem.grid[0, 0];

        if (MushroomFactory.CanPlaceMushroomOnTile(mushroomId, tile.tileType.ToString()))
        {
            activeMushroom = MushroomManager.CreateMushroom(mushroomId, tile.worldPosition, tile);
            tile.go = activeMushroom.gameObject;
            tile.isOccupied = true;
        }
        else
        {
            Debug.Log($"No se puede colocar {mushroomId} en un tile de tipo {tile.tileType}");
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
                Debug.Log("Avanzó de etapa.");
            }
        }
    }
    #endregion
}