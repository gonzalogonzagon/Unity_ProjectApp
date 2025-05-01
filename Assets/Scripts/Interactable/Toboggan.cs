using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Toboggan : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private Sprite tobogganImage;
    [SerializeField] private Image imageDisplayComponent;
    
    public void Interact()
    {
        if (imagePanel != null)
        {
            imagePanel.SetActive(true);
            
            if (imageDisplayComponent != null && tobogganImage != null)
            {
                imageDisplayComponent.sprite = tobogganImage;
                imageDisplayComponent.preserveAspect = true;
            }
        }
        else
        {
            Debug.LogError("No se ha asignado un panel de imagen en " + gameObject.name);
        }
    }
    
    public bool CanInteract()
    {
        return enabled && gameObject.activeInHierarchy;
    }
}