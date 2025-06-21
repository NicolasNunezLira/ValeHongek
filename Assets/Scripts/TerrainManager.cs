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
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        tileGO = Instantiate(tilePrefab);
        tileGO.SetActive(false);

        tileSystem = new TilesSystem(width, height, sizePrefab, tilePrefab);
        tileSystem.GenerateGrid();

        Tile tile = tileSystem.grid[0, 0];

         if (MushroomFactory.CanPlaceMushroomOnTile(mushroomId, tile.tileType.ToString()))
        {
            GameObject mushroom = MushroomFactory.CreateMushroom(mushroomId, tile.worldPosition);
            tile.go = mushroom;
            tile.isOccupied = true;
        }
        else
        {
            Debug.Log($"No se puede colocar {mushroomId} () en un tile de tipo {tile.tileType}");
        }
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}