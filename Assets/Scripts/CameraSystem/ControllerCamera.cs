using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 14f;
    public float zoomSpeed = 100f;
    public float rotationSpeed = 2f;
    public float minZoom = 10f;
    public float maxZoom = 40f;

    private float targetHeight;
    private float currentZoom;
    private bool isTopDown = false;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Start()
    {
        currentZoom = Camera.main.transform.eulerAngles.x;
        targetHeight = Camera.main.transform.position.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCameraView();
        }

        if (isTopDown)
        {
            HandleTopDownMovement();
            HandleMouseDragMovement();
        }
        else
        {
            HandleMovement();
            HandleRotation();
            //HandleZoom();
        }
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (transform.forward * v + transform.right * h).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // clic derecho
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.Rotate(0f, rotX, 0f, Space.World);

            float moveY = Input.GetAxis("Mouse Y") * rotationSpeed * 0.5f; // puedes ajustar el factor
            // Mover en la dirección hacia adelante (pero sin cambiar altura)
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();

            transform.position -= forward * moveY;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetHeight -= scroll * zoomSpeed * Time.deltaTime;
            targetHeight = Mathf.Clamp(targetHeight, minZoom, maxZoom);
        }

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, targetHeight, Time.deltaTime * 5f);
        transform.position = pos;
    }

    void ToggleCameraView()
    {
        if (!isTopDown)
        {
            // Guardar posición/rotación actuales
            savedPosition = transform.position;
            savedRotation = transform.rotation;

            // Subir cámara y alinearla a top-down
            Vector3 topDownPos = savedPosition;
            topDownPos.y = maxZoom; // o un valor fijo como 60f

            transform.position = topDownPos;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            isTopDown = true;
        }
        else
        {
            // Restaurar vista libre
            transform.position = savedPosition;
            transform.rotation = savedRotation;

            isTopDown = false;
        }
    }


    void HandleTopDownMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Movimiento en X-Z
        Vector3 move = new Vector3(h, 0, v).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleMouseDragMovement()
    {
        if (!isTopDown) return; // ← Esto lo bloquea fuera del modo top-down

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            dragOrigin = Input.mousePosition;

            Vector3 move = new Vector3(-difference.x, 0, -difference.y) * moveSpeed;
            transform.Translate(move, Space.World);
        }
    }


}
