using UnityEngine;

public class SceneTransitionButton : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject currentFocusObject;
    [SerializeField] private GameObject nextFocusObject;

    [SerializeField] private RTouchManager2D touchManager;

    public void Interact()
    {
        // Desactiva el actual
        if (currentFocusObject != null)
        {
            currentFocusObject.SetActive(false);
        }

        // Activa el nuevo
        if (nextFocusObject != null)
        {
            nextFocusObject.SetActive(true);

            // Actualiza el target en el touch manager
            touchManager.SetTargetContent(nextFocusObject.transform);

        }
    }

    public bool CanInteract()
    {
        // Puedes añadir lógica adicional si lo necesitas
        return true;
    }
}