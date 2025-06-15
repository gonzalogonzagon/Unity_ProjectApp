using UnityEngine;
using TMPro;

public class InfoMarker3 : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text descriptionField;
    [TextArea] public string infoDescription = "Este es un punto de interés.";
    [SerializeField] private GameObject canvasToShow;

    public CanvasModalTextPages modalTextPages;

    public void Interact()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);

        if (descriptionField != null)
            descriptionField.text = infoDescription;
        else
            Debug.LogWarning("No se ha asignado el campo de descripción a InfoMarker3.");

        CambiarContenido(infoDescription);
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

    public void CambiarContenido(string nuevoTexto)
    {
        if (modalTextPages != null)
        {
            modalTextPages.longText = nuevoTexto;
            modalTextPages.PaginateText();
            modalTextPages.ShowPage(0);
        }
        else
        {
            Debug.LogWarning("No se ha asignado la referencia a CanvasModalTextPages en InfoMarker3.");
        }
    }
}