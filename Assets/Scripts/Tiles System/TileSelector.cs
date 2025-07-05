using UnityEngine;
// Colocarlo en un GameController o Camera Controller

public class TileSelector : MonoBehaviour
{
    [Header("Tile Collider Layer Mask")]
    public LayerMask tileLayerMask;
    private TileHighlighter currentTile;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
        {
            TileHighlighter tile = hit.collider.GetComponentInParent<TileHighlighter>();

            if (tile != null)
            {
                if (currentTile != tile)
                {
                    if (currentTile != null)
                        currentTile.Unhighlight();

                    tile.Highlight();
                    currentTile = tile;
                }
            }
            else
            {
                if (currentTile != null)
                {
                    currentTile.Unhighlight();
                    currentTile = null;
                }
            }
        }
        else
        {
            if (currentTile != null)
            {
                currentTile.Unhighlight();
                currentTile = null;
            }
        }
    }
}
