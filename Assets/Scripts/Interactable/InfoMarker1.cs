using UnityEngine;
using TMPro;

public class InfoMarker1 : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text titleField;       // Campo para el título
    [SerializeField] private TMP_Text descriptionField; // Campo para la descripción
    [SerializeField] private string infoTitle = "Información";
    [SerializeField] private string infoDescription = "Este es un punto de interés.";
    [SerializeField] private GameObject canvasToShow;

    public void Interact()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);
            
        if (titleField != null)
            titleField.text = infoTitle;
        else
            Debug.LogWarning("No se ha asignado el campo de título a InfoMarker1.");

        if (descriptionField != null)
            descriptionField.text = infoDescription;
        else
            Debug.LogWarning("No se ha asignado el campo de descripción a InfoMarker1.");
    }

    public bool CanInteract()
    {
        return true;
    }

    public string getInfoDescription()
    {
        return infoDescription;
    }
    public void setInfoDescription(string description)
    {
        infoDescription = description;
        if (descriptionField != null)
            descriptionField.text = infoDescription;
    }
}