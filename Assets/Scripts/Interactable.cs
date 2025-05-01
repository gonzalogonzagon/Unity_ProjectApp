using UnityEngine;

public interface IInteractable
{
    // Método principal para manejar la interacción
    void Interact();

    // Método para verificar si se puede interactuar con el objeto
    bool CanInteract();

}
