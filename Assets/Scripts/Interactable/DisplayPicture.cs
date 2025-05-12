using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayPicture : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasImagePanel;
    [SerializeField] private Image canvasImageDisplay;
    [SerializeField] private Sprite imageAsset;
    
    public void Interact()
    {
        if (canvasImagePanel != null)
        {
            canvasImagePanel.gameObject.SetActive(true);
            
            if (canvasImageDisplay != null && imageAsset != null)
            {
                canvasImageDisplay.sprite = imageAsset;
                canvasImageDisplay.preserveAspect = true;
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