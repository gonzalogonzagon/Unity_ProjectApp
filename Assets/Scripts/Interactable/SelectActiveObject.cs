using UnityEngine;
using System.Collections.Generic;

public class SelectActiveObject : MonoBehaviour, IInteractable
{
    [Tooltip("Lista de objetos que se pueden activar/desactivar")]
    public List<GameObject> objectsToManage;

    // Llama este método desde el botón, pasando el índice correspondiente
    public void ActivateObjectByIndex(int index)
    {
        for (int i = 0; i < objectsToManage.Count; i++)
        {
            if (objectsToManage[i] != null)
                objectsToManage[i].SetActive(i == index);
        }
    }

    // Ejemplo de implementación de la interfaz
    public void Interact()
    {
        // Puedes dejarlo vacío o implementar lógica adicional si lo necesitas
    }

    public bool CanInteract()
    {
        return true;
    }
}