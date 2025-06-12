using UnityEngine;
using TMPro;

public class TextMarker : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text textField; // Asigna el campo de texto en el Inspector
    [SerializeField] private string message = "Texto de marcador"; // Texto a mostrar

    public void Interact()
    {
        if (textField != null)
        {
            textField.text = message;
        }
        else
        {
            Debug.LogWarning("No se ha asignado el campo de texto a TextMarker.");
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}