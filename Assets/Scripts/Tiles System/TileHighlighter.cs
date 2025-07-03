using FungiSystem;
using TilesManager;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public Tile tile;
    public float liftHeight = 0.3f;
    private Vector3 originalPosition;
    private Vector3? originalMushroomPosition;
    private Renderer borderRenderer;
    private Color originalEmission;
    private readonly Color highlightEmission = Color.white;

    void Start()
    {
        originalPosition = transform.position;

        var border = transform.Find("Boundary_Hexagono/default");
        if (border != null)
        {
            borderRenderer = border.GetComponent<Renderer>();
            if (borderRenderer != null)
            {
                // Instanciar material
                borderRenderer.material = new Material(borderRenderer.material);

                // Asegurar que el color original no sea vacío
                if (borderRenderer.material.HasProperty("_EmissionColor"))
                {
                    originalEmission = borderRenderer.material.GetColor("_EmissionColor");
                }
                else
                {
                    originalEmission = Color.black;
                }

                // Por seguridad, desactiva la emisión inicial si no se usaba
                borderRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void Highlight()
    {
        transform.position = originalPosition + Vector3.up * liftHeight;

        if (borderRenderer != null)
        {
            borderRenderer.material.EnableKeyword("_EMISSION");
            borderRenderer.material.SetColor("_EmissionColor", highlightEmission);
        }

        //Debug.Log($"tile?.mushroom = {tile?.mushroom!=null}");
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
        transform.position = originalPosition;

        if (borderRenderer != null)
        {
            borderRenderer.material.SetColor("_EmissionColor", originalEmission);

            // Si el color es oscuro, también podemos desactivar la emisión
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
