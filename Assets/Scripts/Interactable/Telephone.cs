using UnityEngine;
using System.Collections;

public class Telephone : MonoBehaviour, IInteractable
{
    public AudioSource audioSource;
    public GameObject marker;
    private bool isPlaying = false;

    public void Interact()
    {
        if (CanInteract() && audioSource != null)
        {
            SetMarkerVisible(false);
            audioSource.Play();
            isPlaying = true;
            StartCoroutine(ShowMarkerWhenAudioEnds());
        }
    }

    public bool CanInteract()
    {
        return !isPlaying && audioSource != null && !audioSource.isPlaying;
    }

    private IEnumerator ShowMarkerWhenAudioEnds()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        SetMarkerVisible(true);
        isPlaying = false;
    }

    private void SetMarkerVisible(bool visible)
    {
        var renderer = marker.GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.enabled = visible;
    }
}