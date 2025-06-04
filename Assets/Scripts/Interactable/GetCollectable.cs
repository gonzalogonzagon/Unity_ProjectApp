using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCollectable : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasImagePanel;
    [SerializeField] private Image canvasImageDisplay;
    [SerializeField] private Sprite imageAsset;
    [SerializeField] private string key;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        if (!string.IsNullOrEmpty(key) && PlayerPrefs.GetInt(key) != 0)
        {
            if (imageAsset != null)
            {
                spriteRenderer.sprite = imageAsset;
            }
        }
    }

    public void Interact()
    {
        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.SetInt(key, 1);

            if (spriteRenderer != null && imageAsset != null)
            {
                spriteRenderer.sprite = imageAsset;
            }
        } 
        else 
        {
            Debug.Log("Ya has recogido este coleccionable: " + key);
            return;
        }

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
        bool collected = PlayerPrefs.GetInt(key) == 0 ? false : true;
        return !collected && enabled && gameObject.activeInHierarchy;
    }
}
