using UnityEngine;
// Colocarlo en un GameController o Camera Controller

public class TileSelector : MonoBehaviour
{
    private TileHighlighter currentTile;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
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
