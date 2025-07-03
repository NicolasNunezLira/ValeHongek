using UnityEngine;
using System;
using TilesManager;

namespace FungiSystem
{
    [Serializable]
    public class Mushroom
    {
        // Prefabs por etapa
        public GameObject primordioPrefab;
        public GameObject jovenPrefab;
        public GameObject adultoPrefab;
        public GameObject muriendoPrefab;

        // Necesidades del ambiente
        public Vector2 rangoHumedadIdeal; // Ej: (0.6, 0.9)
        public SubstrateType substrate;
        public EcologicalRelation relacion;
        public string arbolSimbiotico; // opcional

        // Tiempos por etapa
        public float tiempoPrimordio;
        public float tiempoJoven;
        public float tiempoAdulto;

        // Estado
        private MushroomStage etapaActual;
        private float tiempoEnEtapa;
        public bool viva = true;

        // Tile y mundo
        public Vector2Int posicionEnGrid;
        private GameObject instanciaActual;

        public GameObject Instancia => instanciaActual;

        // Constructor
        public Mushroom(
            GameObject primordio, GameObject joven, GameObject adulto, GameObject muriendo,
            Vector2 humedadIdeal, SubstrateType sustrato, EcologicalRelation relacion,
            string arbolSimbiotico, float tPrimordio, float tJoven, float tAdulto
        )
        {
            primordioPrefab = primordio;
            jovenPrefab = joven;
            adultoPrefab = adulto;
            muriendoPrefab = muriendo;
            rangoHumedadIdeal = humedadIdeal;
            substrate = sustrato;
            this.relacion = relacion;
            this.arbolSimbiotico = arbolSimbiotico;
            tiempoPrimordio = tPrimordio;
            tiempoJoven = tJoven;
            tiempoAdulto = tAdulto;
        }

        // Inicializa el hongo en una posición del grid
        public void Initialize(Vector3 posicionMundo)
        {
            etapaActual = MushroomStage.Primordio;
            tiempoEnEtapa = 0f;
            instanciaActual = UnityEngine.Object.Instantiate(primordioPrefab, posicionMundo, Quaternion.identity);
        }

        // Actualiza el crecimiento por tiempo
        public void UpdateGrowth(float deltaTime)
        {
            if (!viva) return;

            tiempoEnEtapa += deltaTime;

            switch (etapaActual)
            {
                case MushroomStage.Primordio:
                    if (tiempoEnEtapa >= tiempoPrimordio)
                        AvanzarEtapa(MushroomStage.Joven);
                    break;

                case MushroomStage.Joven:
                    if (tiempoEnEtapa >= tiempoJoven)
                        AvanzarEtapa(MushroomStage.Adulto);
                    break;

                case MushroomStage.Adulto:
                    if (tiempoEnEtapa >= tiempoAdulto)
                        AvanzarEtapa(MushroomStage.Muriendo);
                    break;

                case MushroomStage.Muriendo:
                    Die();
                    break;
            }
        }

        private void AvanzarEtapa(MushroomStage nuevaEtapa)
        {
            etapaActual = nuevaEtapa;
            tiempoEnEtapa = 0f;
            ChangeStagePrefab();
        }

        private void ChangeStagePrefab()
        {
            if (instanciaActual != null)
                UnityEngine.Object.Destroy(instanciaActual);

            GameObject nuevoPrefab = etapaActual switch
            {
                MushroomStage.Joven => jovenPrefab,
                MushroomStage.Adulto => adultoPrefab,
                MushroomStage.Muriendo => muriendoPrefab,
                _ => primordioPrefab,
            };

            instanciaActual = UnityEngine.Object.Instantiate(nuevoPrefab, instanciaActual.transform.position, Quaternion.identity);
        }

        public void Die()
        {
            viva = false;
            // Aquí puedes lanzar partículas, efectos o sonido
            UnityEngine.Object.Destroy(instanciaActual, 2f); // Destruye después de 2 segundos
        }

        // Evaluar si el hongo puede crecer en el tile actual y vecinos
        public static bool CheckConditions(Tile tile, Tile[] vecinos, Mushroom especie)
        {
            bool humedadOk = tile.humedad >= especie.rangoHumedadIdeal.x && tile.humedad <= especie.rangoHumedadIdeal.y;
            bool sustratoOk = tile.sustrato == especie.substrate;

            bool simbiosisOk = true;
            if (especie.relacion == EcologicalRelation.Simbiotica)
            {
                simbiosisOk = false;
                foreach (Tile t in vecinos)
                {
                    if (t.arbol == especie.arbolSimbiotico)
                    {
                        simbiosisOk = true;
                        break;
                    }
                }
            }

            return humedadOk && sustratoOk && simbiosisOk;
        }

        public void Harvest()
        {
            if (!viva) return;

            // lógica de recolección, sumar puntos, recursos, etc.
            Die();
        }
    }
}
