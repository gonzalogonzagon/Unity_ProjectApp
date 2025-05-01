using UnityEngine;

public class Marker1 : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject targetToActivate;
    [SerializeField] private bool hideOnInteract = true;  // Opción para ocultar este objeto al interactuar
    
    private void Start()
    {
        // Opcionalmente, puedes buscar el objeto por nombre si no está asignado en el inspector
        if (targetToActivate == null) {
            Debug.LogWarning("No se encontró el objeto targetToActive. Asígnalo en el Inspector.", this);
        }
    }
    
    // Implementación de IInteractable.Interact()
    public void Interact()
    {
        Debug.Log($"{gameObject.name} fue interactuado!");
        
        // 1. Habilitar la visibilidad del objeto objetivo
        if (targetToActivate != null)
        {
            targetToActivate.SetActive(true);
            Debug.Log($"Se activó {targetToActivate.name}");
        }
        
        // 2. Deshabilitar nuestra propia visibilidad
        if (hideOnInteract)
        {
            // Opción 1: Ocultar todo el GameObject
            gameObject.SetActive(false);
            
            // Opción 2: Alternativa - solo ocultar el renderer si quieres mantener la funcionalidad
            // GetComponent<Renderer>().enabled = false;
            
            Debug.Log($"{gameObject.name} se ha ocultado");
        }
    }
    
    // Implementación de IInteractable.CanInteract()
    public bool CanInteract()
    {
        // Puedes añadir condiciones adicionales si es necesario
        return enabled && gameObject.activeInHierarchy;
    }
}