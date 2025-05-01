using UnityEngine;

public class MarkerInteraction : MonoBehaviour
{
    [SerializeField] private Color highlightColor = new Color(1, 1, 0, 1);
    private Color originalColor;
    private MeshRenderer meshRenderer;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            originalColor = meshRenderer.material.color;
        }
    }
    
    // Método público que puede ser llamado cuando se toca el marcador
    public void OnMarkerTouched()
    {
        if (meshRenderer != null && meshRenderer.material != null)
        {
            // Cambiar color o realizar alguna acción visual
            meshRenderer.material.color = highlightColor;
            
            // Volver al color original después de un tiempo
            Invoke("ResetColor", 0.5f);
        }
        
        // Aquí puedes añadir más comportamientos (mostrar UI, reproducir sonido, etc.)
    }
    
    private void ResetColor()
    {
        if (meshRenderer != null && meshRenderer.material != null)
        {
            meshRenderer.material.color = originalColor;
        }
    }
}