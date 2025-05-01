using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class UIElementInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onElementInteraction;
    [SerializeField] private bool autoSizeCollider = true;
    
    private RectTransform rectTransform;
    private BoxCollider boxCollider;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider>();
        
        if (autoSizeCollider && boxCollider != null && rectTransform != null)
        {
            // Usar el tamaño real del rectTransform
            Vector2 size = rectTransform.rect.size;
            boxCollider.size = new Vector3(size.x, size.y, 0.1f);
            boxCollider.center = Vector3.zero;
            boxCollider.isTrigger = false;
        }
    }
    
    public void Interact()
    {
        Debug.Log("Interactuando con elemento UI: " + gameObject.name);
        onElementInteraction?.Invoke();
        
        // Si el elemento es un botón, simulamos un clic
        Button button = GetComponent<Button>();
        if (button != null && button.interactable)
        {
            button.onClick.Invoke();
        }
    }
    
    public bool CanInteract()
    {
        // Para botones, comprobamos si están interactables
        Button button = GetComponent<Button>();
        if (button != null)
        {
            return enabled && gameObject.activeInHierarchy && button.interactable;
        }
        
        return enabled && gameObject.activeInHierarchy;
    }
}