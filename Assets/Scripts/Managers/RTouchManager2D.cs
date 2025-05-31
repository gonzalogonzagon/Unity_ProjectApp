using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RTouchManager2D : MonoBehaviour
{
    public static RTouchManager Instance { get; private set; }

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction pinchActionFinger1;
    private InputAction pinchActionFinger2;
    private Camera mainCamera;

    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    private float distance = 50f;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Transform targetContent;

    private Vector2 initialTouchPosition;
    private Vector3 initialCameraPosition;
    private bool isDragging;
    private float touchStartTime;
    [SerializeField]
    private float dragTreshold = 0.2f;

    private float minX, maxX, minY, maxY;

    private bool isZooming = false;
    [SerializeField]
    private float pinchThreshold = 5f; // Ajusta este valor según sea necesario
    [SerializeField]
    private float cameraSpeed = 15f; // Ajusta la velocidad de zoom según sea necesario
    [SerializeField]
    private Transform targetToScale; // Asigna aquí el objeto a escalar (por ejemplo, el sprite)
    [SerializeField]
    private float minScale = 0.5f;
    [SerializeField]
    private float maxScale = 3f;

    private Coroutine zoomCoroutine;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
        pinchActionFinger1 = playerInput.actions["PinchFinger1"];
        pinchActionFinger2 = playerInput.actions["PinchFinger2"];
        mainCamera = Camera.main;

        // Calcular límites de movimiento basados en el sprite
        if (targetContent != null)
        {
            SpriteRenderer sr = targetContent.GetComponent<SpriteRenderer>();
            Vector2 spriteSize = sr.bounds.size;
            float vertExtent = mainCamera.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            minX = targetContent.position.x - spriteSize.x / 2 + horzExtent;
            maxX = targetContent.position.x + spriteSize.x / 2 - horzExtent;
            minY = targetContent.position.y - spriteSize.y / 2 + vertExtent;
            maxY = targetContent.position.y + spriteSize.y / 2 - vertExtent;
        }
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnTouchStarted;
        touchPressAction.canceled += OnTouchEnded;
        touchPositionAction.performed += OnTouchMoved;
        pinchActionFinger1.performed += ZoomStart;
        pinchActionFinger1.canceled += ZoomEnd;
        pinchActionFinger2.performed += ZoomEnd;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchStarted;
        touchPressAction.canceled -= OnTouchEnded;
        touchPositionAction.performed -= OnTouchMoved;
        pinchActionFinger1.performed -= ZoomStart;
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        initialTouchPosition = touchPositionAction.ReadValue<Vector2>();
        initialCameraPosition = mainCamera.transform.position;
        isDragging = true;
        touchStartTime = Time.time;
    }

    private void OnTouchMoved(InputAction.CallbackContext context)
    {
        if (!isDragging) return;

        Vector2 currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
        Vector2 delta = currentTouchPosition - initialTouchPosition;

        // Convertir el delta de pantalla a unidades del mundo
        Vector3 worldDelta = mainCamera.ScreenToWorldPoint(new Vector3(currentTouchPosition.x, currentTouchPosition.y, mainCamera.nearClipPlane))
                           - mainCamera.ScreenToWorldPoint(new Vector3(initialTouchPosition.x, initialTouchPosition.y, mainCamera.nearClipPlane));

        Vector3 newPosition = initialCameraPosition - new Vector3(worldDelta.x, worldDelta.y, 0);

        // Limitar la posición de la cámara
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        mainCamera.transform.position = newPosition;
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        isDragging = false;
        ZoomEnd();

        float touchDuration = Time.time - touchStartTime;

        if (touchDuration < dragTreshold)
            HandleTouch(initialTouchPosition);
    }

    private void HandleTouch(Vector2 touchPos)
    {
        if (IsPointerOverUI(touchPos)) return;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(touchPos);

        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                interactable.Interact();
            }
            else if (hit.collider.gameObject.GetComponent<Billboard>() != null)
            {
                Debug.Log("¡Tocaste un Billboard sin componente de interacción!");
            }
            else
            {
                Debug.Log("¡Tocaste un objeto que no es interactuable!");
            }
        }
    }

    private bool IsPointerOverUI(Vector2 touchPosition) 
    {
        if (EventSystem.current == null) {
            Debug.LogError("EventSystem not found in the scene.");
            return false;
        }

        if (pointerEventData == null) pointerEventData = new PointerEventData(EventSystem.current);

        pointerEventData.position = touchPosition;
        raycastResults.Clear();
    
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }

    private void ZoomStart(InputAction.CallbackContext context) {
        if (zoomCoroutine == null) {
            isZooming = true;
            zoomCoroutine = StartCoroutine(ZoomDetection());
        }
    }

    private void ZoomEnd() {
        if (zoomCoroutine != null) {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
            isZooming = false;
        }
    }

    private void ZoomEnd(InputAction.CallbackContext context) {
        ZoomEnd();
    }

    private IEnumerator ZoomDetection()
    {
        Vector2 touch0Position = pinchActionFinger1.ReadValue<Vector2>();
        Vector2 touch1Position = pinchActionFinger2.ReadValue<Vector2>();
        float previousDistance = Vector2.Distance(touch0Position, touch1Position);
        float currentDistance = 0f;

        while (true)
        {
            touch0Position = pinchActionFinger1.ReadValue<Vector2>();
            touch1Position = pinchActionFinger2.ReadValue<Vector2>();
            currentDistance = Vector2.Distance(touch0Position, touch1Position);

            // Comparar con la distancia previa para determinar el gesto
            if (Mathf.Abs(currentDistance - previousDistance) > pinchThreshold)
            {
                float delta = currentDistance - previousDistance;

                // Calcula el nuevo factor de escala
                float scaleChange = 1 + (delta * cameraSpeed);
                Vector3 newScale = targetToScale.localScale * scaleChange;

                // Limita la escala
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = 1f; // Mantén Z en 1 si es 2D

                targetToScale.localScale = newScale;
                UpdateCameraBounds();
            }
            // Actualizar la distancia previa
            previousDistance = currentDistance;

            yield return null; // Esperar un frame antes de la siguiente iteración
        }
    }

    private void UpdateCameraBounds()
    {
        if (targetContent != null)
        {
            SpriteRenderer sr = targetContent.GetComponent<SpriteRenderer>();
            Vector2 spriteSize = sr.bounds.size;
            float vertExtent = mainCamera.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            minX = targetContent.position.x - spriteSize.x / 2 + horzExtent;
            maxX = targetContent.position.x + spriteSize.x / 2 - horzExtent;
            minY = targetContent.position.y - spriteSize.y / 2 + vertExtent;
            maxY = targetContent.position.y + spriteSize.y / 2 - vertExtent;
        }
    }

    public void SetTargetContent(Transform newTarget)
    {
        targetContent = newTarget;
        targetToScale = newTarget;
        UpdateCameraBounds();

        // Centra la cámara en el nuevo target
        if (mainCamera != null && targetContent != null)
        {
            Vector3 targetPos = targetContent.position;
            mainCamera.transform.position = new Vector3(targetPos.x, targetPos.y, mainCamera.transform.position.z);
        }
    }

}