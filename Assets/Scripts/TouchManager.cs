using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; private set; }

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private Camera mainCamera;

    [SerializeField]
    private float distance = 10f;
    [SerializeField]
    private LayerMask mask;

    private void Awake() {
        // Obtener PlayerInput si existe
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null) {
            Debug.LogError("PlayerInput component not found on this GameObject.");
        }

        // Inicializar acciones de entrada
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];

        // Obtener la cámara principal
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnTouchStarted;
        touchPressAction.canceled += OnTouchEnded;
        touchPositionAction.performed += OnTouchMoved;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchStarted;
        touchPressAction.canceled -= OnTouchEnded;
        touchPositionAction.performed -= OnTouchMoved;
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        Vector2 touchPos = touchPositionAction.ReadValue<Vector2>();
        Debug.Log("Touch Position: " + touchPos);

        HandleTouch(touchPos);
    }

    private void OnTouchMoved(InputAction.CallbackContext context) { /*...*/ }

    private void OnTouchEnded(InputAction.CallbackContext context) { /*...*/ }

    private void HandleTouch(Vector2 touchPos)
    {
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
}
