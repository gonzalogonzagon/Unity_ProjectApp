using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator doorAnimator; // Referencia al Animator que controla la animación
    [SerializeField] private GameObject infoMCloseDoor; // Referencia al objeto InfoMCloseDoor
    [SerializeField] private GameObject infoMOpenDoor; // Referencia al objeto InfoMOpenDoor

    public void Interact()
    {
        Debug.Log("Interacción detectada con InfoMOpenDoor.");

        // Desactivar Marcadores OpenDoor
        gameObject.SetActive(false);
        if (infoMOpenDoor != null)
        {
            infoMOpenDoor.SetActive(false);
        }
        else
        {
            Debug.LogError("No se ha asignado InfoMOpenDoor en el script OpenDoor.");
        }


        // Activar InfoMCloseDoor
        if (infoMCloseDoor != null)
        {
            infoMCloseDoor.SetActive(true);
        }
        else
        {
            Debug.LogError("No se ha asignado InfoMCloseDoor en el script OpenDoor.");
        }

        // Ejecutar la animación de abrir la puerta
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", false); // Cambia "IsOpen" por el nombre real del parámetro en tu Animator
        }
        else
        {
            Debug.LogError("No se ha asignado un Animator al script OpenDoor.");
        }
    }

    public bool CanInteract()
    {
        // Siempre se puede interactuar con este objeto
        return true;
    }
}