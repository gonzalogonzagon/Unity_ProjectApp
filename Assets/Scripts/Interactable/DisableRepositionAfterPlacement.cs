using UnityEngine;
using Vuforia;

public class DisableRepositionAfterPlacement : MonoBehaviour
{
    
    [SerializeField] public PlaneFinderBehaviour planeFinder;
    [SerializeField] public ContentPositioningBehaviour contentPositioning;
    [SerializeField] public GameObject placedObject;

    private bool hasPlaced = false;

    void Start()
    {
        planeFinder.OnInteractiveHitTest.AddListener(OnPlaced);
    }

    void OnPlaced(HitTestResult result)
    {
        if (hasPlaced)
            return;

        hasPlaced = true;

        // Desactiva el posicionador de contenido para evitar más reposicionamientos
        if (contentPositioning != null)
            contentPositioning.enabled = false;

        // Opcional: desactiva el visualizador de plano
        planeFinder.enabled = false;
        planeFinder.gameObject.SetActive(false);

        if (placedObject != null) {
            if (!placedObject.activeSelf)
            {
                placedObject.SetActive(true);
            }
        }
    }

    public void ResetPlacement()
    {
        hasPlaced = false;

        if (contentPositioning != null)
            contentPositioning.enabled = true;

        if (planeFinder != null)
        {
            planeFinder.enabled = true;
            planeFinder.gameObject.SetActive(true);
        }

        if (placedObject != null)
            placedObject.SetActive(false);
    }
}