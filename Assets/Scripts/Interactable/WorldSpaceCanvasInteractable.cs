using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class WorldSpaceCanvasInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;
    
    // Opcional: Ajustar automáticamente el BoxCollider al tamaño del Canvas
    [SerializeField] private bool autoFitCollider = true;
    
    private BoxCollider boxCollider;
    private Canvas canvas;
    
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        boxCollider = GetComponent<BoxCollider>();
        
        if (autoFitCollider && boxCollider != null && canvas != null)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            // Usar el tamaño real del rectTransform
            Vector2 size = rectTransform.rect.size;
            boxCollider.size = new Vector3(size.x, size.y, 0.1f);
            boxCollider.center = Vector3.zero;
            boxCollider.isTrigger = false;
            
            Debug.Log($"Canvas {gameObject.name}: Collider ajustado a tamaño {boxCollider.size} con centro {boxCollider.center}");
        }
    }
    
    public void Interact()
    {
        Debug.Log($"Canvas {gameObject.name} interactuado");
        onInteract?.Invoke();
    }
    
    public bool CanInteract()
    {
        return enabled && gameObject.activeInHierarchy;
    }

    // Helper para visualizar el collider
    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            Gizmos.matrix = originalMatrix;
        }
    }
}