using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RTouchManager : MonoBehaviour
{
    public static RTouchManager Instance { get; private set; }

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction pinchActionFinger1;
    private InputAction pinchActionFinger2;
    private Camera mainCamera;
    private Transform cameraTransform;

    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    private float distance = 50f;
    [SerializeField]
    private LayerMask mask;

    private Vector2 initialTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isDragging;
    [SerializeField]
    private float rotationSpeed = 0.05f; // Ajusta la velocidad de rotación según sea necesario

    private bool isZooming = false;
    [SerializeField]
    private float pinchThreshold = 10f; // Ajusta este valor según sea necesario
    [SerializeField]
    private float cameraSpeed = 60f; // Ajusta la velocidad de zoom según sea necesario

    private Coroutine zoomCoroutine;

    private void Awake() 
    {
        // Obtener PlayerInput si existe
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            Debug.LogError("PlayerInput component not found on this GameObject.");

        // Inicializar acciones de entrada
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
        pinchActionFinger1 = playerInput.actions["PinchFinger1"];
        pinchActionFinger2 = playerInput.actions["PinchFinger2"];


        // Obtener la cámara principal
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnTouchStarted;
        touchPressAction.canceled += OnTouchEnded;
        touchPositionAction.performed += OnTouchMoved;
        pinchActionFinger1.performed += ZoomStart;
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
        Vector2 touchPos = touchPositionAction.ReadValue<Vector2>();
        Debug.Log("Touch Position: " + touchPos);

        // Is dragging?
        initialTouchPosition = touchPos;
        isDragging = true;

        HandleTouch(touchPos);
    }

    private void OnTouchMoved(InputAction.CallbackContext context)
    {
        if (!isDragging || isZooming) return;

        currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
        Vector2 delta = currentTouchPosition - initialTouchPosition;

        RotateCamera(delta); // Rotar la cámara si no está sobre la UI
        // if (IsPointerOverUI(currentTouchPosition))
        // {

        // }
        // else
        // {
        //     RotateCamera(delta); // Rotar la cámara si no está sobre la UI
        // }

        initialTouchPosition = currentTouchPosition; // Actualizar la posición inicial para el siguiente frame
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        isDragging = false;

        ZoomEnd();
    }

    private void HandleTouch(Vector2 touchPos)
    {
        // Verificar si el toque está sobre la UI
        if (IsPointerOverUI(touchPos)) return;

        // Lanzar un rayo para detectar objetos
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(touchPos);
        
        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            // Buscar cualquier componente que implemente IInteractable
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

    private void RotateCamera(Vector2 delta)
    {
        // Rotar en el eje Y (horizontal) sin restricciones
        cameraTransform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);

        // Obtener la rotación actual en el eje X
        float currentXRotation = cameraTransform.eulerAngles.x;
        if (currentXRotation > 180f) currentXRotation -= 360f; // Ajustar para manejar valores negativos

        // Calcular la nueva rotación en el eje X
        float newXRotation = Mathf.Clamp(currentXRotation + delta.y * rotationSpeed, -80f, 80f); // Limitar entre -80 y 80 grados

        // Aplicar la rotación restringida en el eje X
        Vector3 newRotation = cameraTransform.eulerAngles;
        newRotation.x = newXRotation;
        cameraTransform.eulerAngles = newRotation;
    }
    // private void RotateCamera(Vector2 delta)
    // {
    //     cameraTransform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
    //     cameraTransform.Rotate(Vector3.right, delta.y * rotationSpeed, Space.Self);
    // }

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

    private IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, currentDistance = 0f;

        while (true)
        {
            // Obtener las posiciones de los dos toques
            Vector2 touch0Position = pinchActionFinger1.ReadValue<Vector2>(); // Primer toque
            Vector2 touch1Position = pinchActionFinger2.ReadValue<Vector2>(); // Segundo toque (pinch)

            // Calcular la distancia actual entre los dos toques
            currentDistance = Vector2.Distance(touch0Position, touch1Position);

            // Comparar con la distancia previa para determinar el gesto
            if (previousDistance != 0)
            {
                float delta = currentDistance - previousDistance;

                if (Mathf.Abs(delta) > pinchThreshold) // Ignorar pequeños cambios
                {
                    if (delta < 0)
                    {
                        // Zoom In
                        mainCamera.fieldOfView = Mathf.Max(mainCamera.fieldOfView - cameraSpeed * Time.deltaTime, 20f); // Límite mínimo
                    }
                    else
                    {
                        // Zoom Out
                        mainCamera.fieldOfView = Mathf.Min(mainCamera.fieldOfView + cameraSpeed * Time.deltaTime, 60f); // Límite máximo
                    }
                }
            }

            // Actualizar la distancia previa
            previousDistance = currentDistance;

            yield return null; // Esperar un frame antes de la siguiente iteración
        }
    }

    // private void ZoomIn(float delta)
    // {
    //     // Implementa el comportamiento de Zoom In
    //     mainCamera.fieldOfView = Mathf.Max(mainCamera.fieldOfView - delta * 0.1f, 20f); // Ajusta el límite mínimo
    // }

    // private void ZoomOut(float delta)
    // {
    //     // Implementa el comportamiento de Zoom Out
    //     mainCamera.fieldOfView = Mathf.Min(mainCamera.fieldOfView + delta * 0.1f, 60f); // Ajusta el límite máximo
    // }

    // private void OnPinch(InputAction.CallbackContext context)
    // {
    //     // Obtener la posición del primer toque (touch0)
    //     if (Touchscreen.current.touches.Count < 2) return; // Asegurarse de que haya al menos dos toques activos
    //     Vector2 touch0Position = Touchscreen.current.touches[0].position.ReadValue();

    //     // Obtener la posición del segundo toque (touch1) desde la acción Pinch
    //     Vector2 touch1Position = context.ReadValue<Vector2>();

    //     // Calcular la distancia actual entre los dos toques
    //     float currentDistance = Vector2.Distance(touch0Position, touch1Position);

    //     // Comparar con la distancia previa para determinar el gesto
    //     if (previousPinchDistance != 0)
    //     {
    //         float delta = currentDistance - previousPinchDistance;

    //         if (Mathf.Abs(delta) > pinchThreshold) // Ignorar pequeños cambios
    //         {
    //             if (delta > 0)
    //             {
    //                 ZoomIn(delta); // Zoom In
    //             }
    //             else
    //             {
    //                 ZoomOut(delta); // Zoom Out
    //             }
    //         }
    //     }

    //     // Actualizar la distancia previa
    //     previousPinchDistance = currentDistance;
    // }
}
