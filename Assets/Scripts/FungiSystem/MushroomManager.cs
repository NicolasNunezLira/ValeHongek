using System.Collections.Generic;
using TilesManager;
using UnityEngine;

namespace FungiSystem
{
    public static class MushroomManager
    {
        private static List<MushroomInstance> mushrooms = new List<MushroomInstance>();

    public static MushroomInstance CreateMushroom(string scientificName, Vector3 position, Tile tile)
    {
        var data = MushroomFactory.GetData(scientificName);
        if (data == null)
        {
            Debug.LogError($"Mushroom data for {scientificName} not found.");
            return null;
        }

        string initialStage = "hyphalKnot";
        GameObject prefab = Resources.Load<GameObject>(data.prefabs[initialStage]);
        if (prefab == null)
        {
            Debug.LogError($"Prefab for stage '{initialStage}' not found.");
            return null;
        }

        // Instanciar el hongo en la posición base del tile
        GameObject instanceGO = GameObject.Instantiate(
            prefab,
            position,
            Quaternion.identity
        );
        instanceGO.name = $"{data.commonName}_{initialStage}";

        // Escalar el hongo según el tamaño del tile
        Renderer rend = instanceGO.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            /*
            Vector3 boundsSize = rend.bounds.size;
            float maxDim = Mathf.Max(boundsSize.x, boundsSize.z);

            if (maxDim > 0f)
            {
                float tileSize = tile.go != null ? tile.go.transform.localScale.x : 1f;

                var scaleFactors = new Dictionary<string, float>
                {
                    {"hyphalKnot", 0.5f},
                    {"primordia", 0.7f},
                    {"young", 0.85f},
                    {"adult", 1.0f},
                    {"dying", 0.8f}
                };

                float baseScale = tileSize / maxDim;
                float relativeScale = scaleFactors.ContainsKey(initialStage) ? scaleFactors[initialStage] : 1f;

                instanceGO.transform.localScale *= baseScale * relativeScale;
            }
            */

            // Ajustar altura del hongo para que se apoye sobre el tile
            float yOffset = rend.bounds.min.y;
            float correctedY = position.y + tile.height - yOffset + 0.01f; // 0.01 para evitar z-fighting
            instanceGO.transform.position = new Vector3(position.x, correctedY, position.z);
            
        }
        else
        {
            // Si no tiene renderer, posicionar solo encima del tile
            instanceGO.transform.position = position + Vector3.up * (tile.height + 0.01f);
        }

        // Crear y registrar la instancia del hongo
        var instance = new MushroomInstance(scientificName, initialStage, instanceGO, tile);
        mushrooms.Add(instance);
        return instance;
    }

        /*public static MushroomInstance CreateMushroom(string scientificName, Vector3 position, Tile tile)
        {
            var data = MushroomFactory.GetData(scientificName);
            if (data == null)
            {
                Debug.LogError($"Mushroom data for {scientificName} not found.");
                return null;
            }

            // Usa directamente el prefab inicial
            string initialStage = "hyphalKnot";
            GameObject prefab = Resources.Load<GameObject>(data.prefabs[initialStage]);
            if (prefab == null)
            {
                Debug.LogError($"Prefab for stage '{initialStage}' not found.");
                return null;
            }

            GameObject instanceGO = GameObject.Instantiate(
                prefab,
                position + new UnityEngine.Vector3(0f, tile.height + 0.01f, 0f), //+ new Vector3(0, tile.go.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.y * tile.go.transform.localScale.y, 0),
                Quaternion.identity);
            instanceGO.name = $"{data.commonName}_{initialStage}";

            var instance = new MushroomInstance(scientificName, initialStage, instanceGO, tile);
            mushrooms.Add(instance);
            return instance;
        }*/

        public static void AdvanceAll()
        {
            foreach (var m in mushrooms)
            {
                //m.AdvanceStage();
            }
        }

        public static void ClearAll()
        {
            foreach (var m in mushrooms)
            {
                if (m.gameObject != null)
                    GameObject.Destroy(m.gameObject);
            }
            mushrooms.Clear();
        }

        public static List<MushroomInstance> GetAll()
        {
            return mushrooms;
        }
    }
}
