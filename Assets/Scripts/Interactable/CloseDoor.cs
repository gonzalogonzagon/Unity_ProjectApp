using UnityEngine;

public class CloseDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator doorAnimator; // Referencia al Animator que controla la animación
    [SerializeField] private GameObject infoMOpenDoor; // Referencia al objeto InfoMOpenDoor
    [SerializeField] private GameObject infoMOpenDoor2; // Referencia al objeto InfoMOpenDoor

    public void Interact()
    {
        Debug.Log("Interacción detectada con InfoMCloseDoor.");

        // Desactivar el objeto actual
        gameObject.SetActive(false);

        // Activar Marcadores
        if (infoMOpenDoor != null)
        {
            infoMOpenDoor.SetActive(true);
            infoMOpenDoor2.SetActive(true);
        }
        else
        {
            Debug.LogError("No se ha asignado InfoMOpenDoor en el script CloseDoor.");
        }

        // Ejecutar la animación de cerrar la puerta
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", true);
        }
        else
        {
            Debug.LogError("No se ha asignado un Animator al script CloseDoor.");
        }
    }

    public bool CanInteract()
    {
        // Siempre se puede interactuar con este objeto
        return true;
    }
}