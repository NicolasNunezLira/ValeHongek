using UnityEngine;
using TilesManager;
using FungiSystem;
using System.Collections.Generic;

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
    [SerializeField] public GameObject bigTreePrefab;
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
        tileSystem = new TilesSystem(width, height, sizePrefab, tilePrefab, treePrefab, bigTreePrefab);
        tileSystem.GenerateHexGrid();

        /*Tile tile = tileSystem.grid[0, 0];

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

        Tile tile2 = tileSystem.grid[1, 1];
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
        }*/
        FillRemainingTilesWithMushrooms2();


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

    #region Fill Remaining
    private void FillRemainingTilesWithMushrooms()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Tile tile = tileSystem.grid[x, z];
                if (tile == null || tile.isOccupied || tile.treeGroup != null || tile.bigTreeGroup != null)
                    continue;

                float rand = Random.value;
                string chosenId = rand < 0.5f ? mushroomId : mushroomId2;
                tile.tileType = (rand < 0.5f) ? tile.tileType : TileType.DeadTree;

                if (MushroomFactory.CanPlaceMushroomOnTile(chosenId, tile.tileType.ToString()))
                {
                    MushroomInstance mushroom = MushroomManager.CreateMushroom(chosenId, tile.worldPosition, tile);
                    tile.mushroom = mushroom;
                    tile.isOccupied = true;
                }
            }
        }
    }
    public void FillRemainingTilesWithMushrooms2()
    {
        if (tileSystem == null)
        {
            Debug.LogWarning("TileSystem no está inicializado.");
            return;
        }

        string[] mushroomIds = new string[] { mushroomId, mushroomId2 };

        foreach (Tile tile in tileSystem.grid)
        {
            if (tile == null)
                continue;

            // Solo llenar tiles que no tienen árbol y no están ocupados
            if (tile.treeGroup == null && tile.bigTreeGroup == null && !tile.isOccupied)
            {
                // Elegir aleatoriamente qué hongo usar
                string selectedMushroomId = mushroomIds[Random.Range(0, mushroomIds.Length)];
                var data = MushroomFactory.GetData(selectedMushroomId);
                if (data == null || data.times == null || data.times.Count == 0)
                {
                    Debug.LogWarning($"No hay datos de etapas para el hongo {selectedMushroomId}");
                    continue;
                }

                // Obtener lista de etapas posibles (keys del diccionario)
                var stagesList = new List<string>(data.times.Keys);

                // Elegir etapa inicial aleatoria
                int randomStageIndex = Random.Range(0, stagesList.Count);
                string initialStage = stagesList[randomStageIndex];

                // Posición donde crear el hongo (en el tile)
                Vector3 pos = tile.worldPosition;

                // Crear el prefab del hongo de la etapa inicial
                GameObject prefab = Resources.Load<GameObject>(data.prefabs[initialStage]);
                if (prefab == null)
                {
                    Debug.LogWarning($"Prefab no encontrado para {selectedMushroomId} etapa {initialStage}");
                    continue;
                }

                GameObject mushroomGO = GameObject.Instantiate(prefab, pos, Quaternion.identity);
                mushroomGO.name = $"{selectedMushroomId}_{initialStage}";
                mushroomGO.SetActive(true);

                // Crear la instancia MushroomInstance con la etapa inicial
                MushroomInstance mushroomInstance = new MushroomInstance(selectedMushroomId, initialStage, mushroomGO, tile);

                // Asignar el hongo al tile
                tile.mushroom = mushroomInstance;
                tile.isOccupied = true;
            }
        }
    }

    #endregion
}