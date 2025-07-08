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
        if (tile?.treeGroup != null)
        {
            foreach (var groupedTile in tile.treeGroup.tiles)
            {
                HighlightTile(groupedTile);
            }

            if (tile.treeGroup.treeInstance != null)
            {
                tile.treeGroup.treeInstance.transform.position += Vector3.up * liftHeight;
            }
        }
        else
        {
            HighlightTile(tile);
        }
    }

    public void Unhighlight()
    {
        if (tile?.treeGroup != null)
        {
            foreach (var groupedTile in tile.treeGroup.tiles)
            {
                UnhighlightTile(groupedTile);
            }

            if (tile.treeGroup.treeInstance != null)
            {
                Vector3 pos = tile.treeGroup.treeInstance.transform.position;
                tile.treeGroup.treeInstance.transform.position = new Vector3(pos.x, 0.01f, pos.z);
            }
        }
        else
        {
            UnhighlightTile(tile);
        }
    }


    private void HighlightTile(Tile t)
    {
        if (t.go == null) return;

        Transform tRoot = t.go.transform;
        Transform vis = tRoot.Find("default");
        Transform border = tRoot.Find("Boundary_Hexagono/default");

        if (vis != null)
            vis.localPosition += Vector3.up * liftHeight;

        if (border != null)
        {
            Renderer r = border.GetComponent<Renderer>();
            if (r != null)
            {
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", highlightEmission);
                border.localPosition += Vector3.up * liftHeight;
            }
        }

        if (t.mushroom != null && !t.mushroom.isDead && t.mushroom.gameObject != null)
        {
            t.mushroom.gameObject.transform.position += Vector3.up * liftHeight;
        }
    }

    private void UnhighlightTile(Tile t)
    {
        if (t.go == null) return;

        Transform tRoot = t.go.transform;
        Transform vis = tRoot.Find("default");
        Transform border = tRoot.Find("Boundary_Hexagono/default");

        if (vis != null)
            vis.localPosition -= Vector3.up * liftHeight;

        if (border != null)
        {
            Renderer r = border.GetComponent<Renderer>();
            if (r != null)
            {
                r.material.SetColor("_EmissionColor", originalEmission);
                if (originalEmission.maxColorComponent <= 0.01f)
                {
                    r.material.DisableKeyword("_EMISSION");
                }
                border.localPosition -= Vector3.up * liftHeight;
            }
        }

        if (t.mushroom != null && !t.mushroom.isDead && t.mushroom.gameObject != null)
        {
            t.mushroom.gameObject.transform.position -= Vector3.up * liftHeight;
        }
    }
}
