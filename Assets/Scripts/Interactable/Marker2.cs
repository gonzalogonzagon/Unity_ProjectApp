using UnityEngine;
using System.Collections;

public class Marker2 : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject targetToActivate; // Referencia a "PruebaImagen"
    [SerializeField] private bool hideOnInteract = true;  // Opción para ocultar este objeto al interactuar
    
    [SerializeField] private bool fadeInTarget = false;
    [SerializeField] private bool fadeOutSelf = false;
    [SerializeField] private float fadeSpeed = 2.0f;

    private void Start()
    {
        // Opcionalmente, puedes buscar el objeto por nombre si no está asignado en el inspector
        if (targetToActivate == null)
        {
            targetToActivate = GameObject.Find("PruebaImagen");
            if (targetToActivate == null)
            {
                Debug.LogWarning("No se encontró el objeto 'PruebaImagen (1)'. Asígnalo en el Inspector.", this);
            }
        }
    }
    
    // Implementación de IInteractable.Interact()
    public void Interact()
    {
        Debug.Log($"{gameObject.name} fue interactuado!");
        
        // 1. Habilitar la visibilidad del objeto objetivo
        if (targetToActivate != null)
        {
            if (fadeInTarget)
            {
                // Asegurarse de que está activo pero invisible para poder hacer fade in
                Renderer targetRenderer = targetToActivate.GetComponent<Renderer>();
                if (targetRenderer != null)
                {
                    Color startColor = targetRenderer.material.color;
                    startColor.a = 0f;
                    targetRenderer.material.color = startColor;
                }
                
                targetToActivate.SetActive(true);
                FadeIn(targetToActivate);
            }
            else
            {
                // Comportamiento original sin fundido
                targetToActivate.SetActive(true);
            }
            Debug.Log($"Se activó {targetToActivate.name}");
        }
        
        // 2. Deshabilitar nuestra propia visibilidad
        if (hideOnInteract)
        {
            if (fadeOutSelf)
            {
                // Iniciar efecto de fundido - el objeto se desactivará al final
                FadeOut(gameObject);
            }
            else
            {
                // Comportamiento original sin fundido
                gameObject.SetActive(false);
            }
            
            Debug.Log($"{gameObject.name} se está ocultando");
        }
    }
    
    // Implementación de IInteractable.CanInteract()
    public bool CanInteract()
    {
        // Puedes añadir condiciones adicionales si es necesario
        return enabled && gameObject.activeInHierarchy;
    }



    private void FadeIn(GameObject obj)
    {
        // Implementación de efecto fade-in usando coroutines
        StartCoroutine(FadeRoutine(obj, 0f, 1f, fadeSpeed));
    }

    private void FadeOut(GameObject obj)
    {
        // Implementación de efecto fade-out usando coroutines
        StartCoroutine(FadeRoutine(obj, 1f, 0f, fadeSpeed, true));
    }

    private IEnumerator FadeRoutine(GameObject obj, float startAlpha, float endAlpha, float speed, bool disableAtEnd = false)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Color currentColor = renderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < 1/speed)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime * speed);
            currentColor.a = newAlpha;
            renderer.material.color = currentColor;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentColor.a = endAlpha;
        renderer.material.color = currentColor;
        
        if (disableAtEnd && endAlpha <= 0.01f)
        {
            obj.SetActive(false);
        }
    }
}