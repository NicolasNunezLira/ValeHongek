using UnityEngine;
using System.Collections.Generic;
using TilesManager;

namespace FungiSystem
{
    public class MushroomInstance
    {
        public string scientificName;
        public string stage;
        public GameObject gameObject;
        public Tile currentTile;         // Tile donde está colocada la seta
        public bool isDead = false;      // Estado de muerte

        public MushroomInstance(string name, string stage, GameObject obj, Tile tile)
        {
            this.scientificName = name;
            this.stage = stage;
            this.gameObject = obj;
            this.currentTile = tile;
        }

        public void AdvanceStage(float tileSize)
        {
            var data = MushroomFactory.GetData(scientificName);
            var nextStage = GetNextStage(stage, data);

            // Si no hay siguiente etapa (después de dying), destruir la seta y liberar el tile
            if (nextStage == null || nextStage == "none")
            {
                Debug.Log($"{scientificName} reached final stage: {stage}. Destroying instance.");
                
                if (gameObject != null)
                    GameObject.Destroy(gameObject);

                if (currentTile != null)
                {
                    currentTile.isOccupied = false;
                    currentTile.go = null;
                }

                return;
            }

            stage = nextStage;

            Vector3 originalPos = gameObject.transform.position;
            GameObject.Destroy(gameObject);

            GameObject newPrefab = Resources.Load<GameObject>(data.prefabs[stage]);
            if (newPrefab == null)
            {
                Debug.LogWarning($"No prefab found for stage {stage} of {scientificName}");
                return;
            }

            gameObject = GameObject.Instantiate(newPrefab, originalPos, Quaternion.identity);
            gameObject.name = data.commonName + "_" + stage;

            Renderer rend = gameObject.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                Vector3 size = rend.bounds.size;
                float maxDim = Mathf.Max(size.x, size.z);

                if (maxDim > 0)
                {
                    var scaleFactors = new Dictionary<string, float>
                    {
                        {"hyphalKnot", 0.5f},
                        {"primordia", 0.7f},
                        {"young", 0.85f},
                        {"adult", 1.0f},
                        {"dying", 0.8f}
                    };

                    float baseScale = tileSize / maxDim;
                    float relativeScale = 1f;

                    if (scaleFactors.ContainsKey(stage))
                        relativeScale = scaleFactors[stage];

                    gameObject.transform.localScale = gameObject.transform.localScale * baseScale * relativeScale;
                }

                float yOffset = rend.bounds.min.y;
                Vector3 adjustedPos = new Vector3(
                    originalPos.x,
                    originalPos.y - yOffset,
                    originalPos.z
                );
                gameObject.transform.position = adjustedPos;
            }
            else
            {
                gameObject.transform.position = originalPos;
            }
        }

        private string GetNextStage(string current, MushroomData data)
        {
            if (data == null || data.times == null)
            {
                Debug.LogError("MushroomData or times dictionary is null.");
                return null;
            }

            var stages = new List<string>(data.times.Keys);
            int index = stages.IndexOf(current);
            if (index >= 0 && index < stages.Count - 1)
            {
                return stages[index + 1];
            }
            return null;
        }
    }
}
