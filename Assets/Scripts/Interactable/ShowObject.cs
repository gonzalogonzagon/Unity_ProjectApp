using UnityEngine;

public class ShowObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject targetObject; // Objeto que se activará al interactuar

    [SerializeField]
    private bool hideObject = true; // Determina si el objeto comienza visible o no

    public void Interact()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // Activa el objeto
            Debug.Log(targetObject.name + " ha sido activado.");

            if (hideObject) {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("No se ha asignado un objeto a 'targetObject' en " + gameObject.name);
        }
    }
    
    public bool CanInteract()
    {
        return enabled && gameObject.activeInHierarchy;
    }
}
