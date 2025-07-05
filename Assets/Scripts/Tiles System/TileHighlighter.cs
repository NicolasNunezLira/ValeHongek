using FungiSystem;
using TilesManager;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public Tile tile;
    public float liftHeight = 0.3f;

    private Transform visualTransform, borderTransform; // el hijo "default" visual
    private Vector3 originalVisualPosition, originalBorderPosition;
    private Vector3? originalMushroomPosition;
    private Renderer borderRenderer;
    private Color originalEmission;
    private readonly Color highlightEmission = Color.white;

    void Start()
    {
        // Encuentra el hijo visual
        visualTransform = transform.Find("default");
        if (visualTransform != null)
        {
            originalVisualPosition = visualTransform.localPosition;
        }
        else
        {
            Debug.LogWarning("No se encontró el hijo 'default' para levantar visualmente el tile.");
        }

        // Encuentra el borde
        borderTransform = transform.Find("Boundary_Hexagono/default");
        if (borderTransform != null)
        {
            originalBorderPosition = borderTransform.localPosition;
            borderRenderer = borderTransform.GetComponent<Renderer>();
            if (borderRenderer != null)
            {
                borderRenderer.material = new Material(borderRenderer.material);
                originalEmission = borderRenderer.material.HasProperty("_EmissionColor")
                    ? borderRenderer.material.GetColor("_EmissionColor")
                    : Color.black;
                borderRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void Highlight()
    {
        // Solo levanta el visual
        if (visualTransform != null)
        {
            visualTransform.localPosition = originalVisualPosition + Vector3.up * liftHeight;
        }

        if (borderRenderer != null)
        {
            borderTransform.localPosition = originalBorderPosition + Vector3.up * liftHeight;
            borderRenderer.material.EnableKeyword("_EMISSION");
            borderRenderer.material.SetColor("_EmissionColor", highlightEmission);
        }

        // Levanta el hongo si existe
        if (tile?.mushroom != null && !tile.mushroom.isDead && tile.mushroom.gameObject != null)
        {
            var mushGO = tile.mushroom.gameObject;

            if (!originalMushroomPosition.HasValue)
                originalMushroomPosition = mushGO.transform.position;

            mushGO.transform.position = originalMushroomPosition.Value + Vector3.up * liftHeight;
        }
    }

    public void Unhighlight()
    {
        if (visualTransform != null)
        {
            visualTransform.localPosition = originalVisualPosition;
        }

        if (borderRenderer != null)
        {
            borderTransform.localPosition = originalBorderPosition;
            borderRenderer.material.SetColor("_EmissionColor", originalEmission);

            if (originalEmission.maxColorComponent <= 0.01f)
            {
                borderRenderer.material.DisableKeyword("_EMISSION");
            }
        }

        if (tile?.mushroom != null && !tile.mushroom.isDead && tile.mushroom.gameObject != null && originalMushroomPosition.HasValue)
        {
            tile.mushroom.gameObject.transform.position = originalMushroomPosition.Value;
        }
    }
}
