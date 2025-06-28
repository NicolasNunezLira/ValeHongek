using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public GameObject borderObject; // referencia al borde negro del prefab
    public float liftHeight = 0.2f;
    private Vector3 originalPosition;
    private Vector3 originalBorderPosition;
    private Renderer borderRenderer;
    private Color originalColor;
    private Color highlightColor = Color.white;

    void Start()
    {
        originalPosition = transform.position;

        if (borderObject != null)
        {
            originalBorderPosition = borderObject.transform.position;
            borderRenderer = borderObject.GetComponent<Renderer>();
            originalColor = borderRenderer.material.color;
        }
    }

    /*void OnMouseEnter()
    {
        transform.position = originalPosition + Vector3.up * liftHeight;

        if (borderObject != null)
        {
            borderObject.transform.position = originalBorderPosition + Vector3.up * (liftHeight + 0.01f);
            borderRenderer.material.color = highlightColor;
        }
    }

    void OnMouseExit()
    {
        transform.position = originalPosition;

        if (borderObject != null)
        {
            borderObject.transform.position = originalBorderPosition;
            borderRenderer.material.color = originalColor;
        }
    }*/

    public void Highlight()
    {
        transform.position = originalPosition + Vector3.up * liftHeight;
        if (borderObject != null)
        {
            borderObject.transform.position = originalBorderPosition + Vector3.up * (liftHeight + 0.01f);
            borderRenderer.material.color = highlightColor;
        }
    }

    public void Unhighlight()
    {
        transform.position = originalPosition;
        if (borderObject != null)
        {
            borderObject.transform.position = originalBorderPosition;
            borderRenderer.material.color = originalColor;
        }
    }
}
