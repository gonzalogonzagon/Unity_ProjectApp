using UnityEngine;

public class GroundPlane : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject groundPlaneStage; // Referencia al Ground Plane Stage

    public void Interact()
    {
        Debug.Log("Interacción detectada con el Ground Plane Stage.");

        // Activar los objetos hijos del Ground Plane Stage
        foreach (Transform child in groundPlaneStage.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public bool CanInteract()
    {
        // Verificar si el Ground Plane Stage está activo en la jerarquía
        return groundPlaneStage != null && groundPlaneStage.activeInHierarchy;
    }
}