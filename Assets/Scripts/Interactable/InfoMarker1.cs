using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMarker1 : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject canvasModalText; // Referencia al canvas que se mostrará al interactuar

    [SerializeField]
    private GameObject canvasModalText2; // Referencia al segundo canvas que se mostrará al interactuar (opcional)
    
    [SerializeField]
    private string infoTitle = "Información"; // Título que se mostrará en el canvas
    
    [SerializeField]
    private string infoDescription = "Este es un punto de interés."; // Descripción a mostrar
    
    private bool isInteractable = true; // Determina si el marcador puede ser interactuado

    private void Start()
    {
        // Si no se asigna el canvas en el inspector, intentamos encontrarlo por tag o nombre
        if (canvasModalText == null)
        {
            // Primero buscamos por tag
            canvasModalText = GameObject.FindWithTag("CanvasModalText");
            
            // Si no lo encontramos por tag, buscamos por nombre
            if (canvasModalText == null)
            {
                canvasModalText = GameObject.Find("CanvasModal1");
                
                if (canvasModalText == null)
                {
                    Debug.LogWarning("No se ha asignado un canvas modal al InfoMarker1. Por favor asigna uno en el inspector.");
                }
            }
        }
        
        // Aseguramos que el canvas esté oculto al inicio
        if (canvasModalText != null)
        {
            canvasModalText.SetActive(false);
        }
    }

    // Método de la interfaz IInteractable que se llama cuando el usuario interactúa con este objeto
    public void Interact()
    {
        if (canvasModalText != null)
        {
            // Activamos el canvas modal
            canvasModalText.SetActive(true);
            
            // Si el canvas tiene componentes para mostrar título y descripción, los configuramos
            TMPro.TextMeshProUGUI[] textComponents = canvasModalText2.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach (TMPro.TextMeshProUGUI textComponent in textComponents)
            {
                if (textComponent.name.Contains("Title"))
                {
                    textComponent.text = infoTitle;
                }
                else if (textComponent.name.Contains("Description") || textComponent.name.Contains("Content"))
                {
                    textComponent.text = infoDescription;
                }
            }
            
            Debug.Log("InfoMarker activado: Mostrando información");
        }
        else
        {
            Debug.LogError("No hay un canvas modal asignado al InfoMarker1");
        }
    }

    // Método de la interfaz IInteractable para verificar si se puede interactuar con el objeto
    public bool CanInteract()
    {
        return isInteractable;
    }
    
    // Método para habilitar o deshabilitar la interacción
    public void SetInteractable(bool value)
    {
        isInteractable = value;
    }
}