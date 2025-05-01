using UnityEngine;
using UnityEngine.UI;

public class UIOverlayController : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas; // CanvasUIOverlay
    [SerializeField] private Canvas textTestCanvas; // CanvasUITextTest
    [SerializeField] private Button exitButton; // ButtonExit
    
    private void Awake()
    {
        // Add listener to the exit button
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonPressed);
        }
    }
    
    private void OnExitButtonPressed()
    {
        // Toggle the text test canvas
        if (textTestCanvas != null)
        {
            textTestCanvas.gameObject.SetActive(true);
        }
    }
}