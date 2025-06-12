using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class Film : MonoBehaviour, IInteractable
{
    public VideoPlayer videoPlayer;
    public Button uiButton;
    private bool isPlaying = false;

    public void Interact()
    {
        if (CanInteract() && videoPlayer != null)
        {
            videoPlayer.frame = 0;
            videoPlayer.Play();
            isPlaying = true;
            if (uiButton != null)
                uiButton.gameObject.SetActive(false); // Oculta el botón al interactuar
            StartCoroutine(ShowMarkerWhenVideoEnds());
        }
    }

    public bool CanInteract()
    {
        return !isPlaying && videoPlayer != null && !videoPlayer.isPlaying;
    }

    private IEnumerator ShowMarkerWhenVideoEnds()
    {
        yield return new WaitWhile(() => videoPlayer.isPlaying);
        isPlaying = false;
        if (uiButton != null)
            uiButton.gameObject.SetActive(true); // Muestra el botón al terminar el vídeo
    }
}