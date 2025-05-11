using UnityEngine;
using UnityEngine.InputSystem;
using Vuforia;

public class PlaneFinderNewInputSys : MonoBehaviour
{
    [SerializeField] private GameObject objectToPlace; // Objeto que se colocará en el plano
    [SerializeField] private Camera mainCamera; // Cámara principal
    [SerializeField] private PlaneFinderBehaviour planeFinder; // Referencia al Plane Finder

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void Awake()
    {
        // Obtener las acciones de entrada personalizadas
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on this GameObject.");
            return;
        }

        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        // planeFinder.OnInteractiveHitTest += OnHitTestResult; // Suscribirse al delegado
        // touchPressAction.performed += OnTouchPerformed;
    }

    private void OnDisable()
    {
        // planeFinder.OnInteractiveHitTest -= OnHitTestResult; // Desuscribirse del delegado
        // touchPressAction.performed -= OnTouchPerformed;
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = touchPositionAction.ReadValue<Vector2>();
        Debug.Log("Touch detected at position: " + touchPosition);

        HandleTouch(touchPosition);
    }

    private void HandleTouch(Vector2 touchPosition)
    {
        // Realizar un raycast en el plano detectado por Vuforia
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        if (planeFinder != null)
        {
            // El PlaneFinderBehaviour manejará el hit test automáticamente.
        }
        else
        {
            Debug.LogError("PlaneFinderBehaviour is not assigned.");
        }
    }

    private void OnHitTestResult(HitTestResult result)
    {
        if (result != null)
        {
            Debug.Log("HitTestResult: " + result.Position);

            // Colocar el objeto en la posición detectada
            if (objectToPlace != null)
            {
                objectToPlace.transform.position = result.Position;
                Debug.Log("Object placed at: " + result.Position);
            }
        }
        else
        {
            Debug.Log("No plane detected.");
        }
    }
}