using UnityEngine;
using UnityEngine.UI;

public class CalleAlcala : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSprite; // Referencia al SpriteRenderer del objeto CalleAlcaláActual
    
    // Valores de transparencia predefinidos
    [Range(0f, 1f)]
    [SerializeField] private float invisibleAlpha = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float semiTransparentAlpha = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float fullyVisibleAlpha = 1f;

    private void Start()
    {
        // Asegurarse de que tenemos una referencia válida
        if (targetSprite == null)
        {
            Debug.LogError("No se ha asignado un SpriteRenderer en " + gameObject.name);
            return;
        }

        // Inicialmente establecer como invisible
        SetTransparencyInvisible();
    }

    /// <summary>
    /// Hace el sprite completamente invisible (alpha = 0)
    /// </summary>
    public void SetTransparencyInvisible()
    {
        SetSpriteAlpha(invisibleAlpha);
    }

    /// <summary>
    /// Hace el sprite semi-transparente (alpha = 0.5)
    /// </summary>
    public void SetTransparencySemiVisible()
    {
        SetSpriteAlpha(semiTransparentAlpha);
    }

    /// <summary>
    /// Hace el sprite completamente visible (alpha = 1)
    /// </summary>
    public void SetTransparencyFullyVisible()
    {
        SetSpriteAlpha(fullyVisibleAlpha);
    }

    /// <summary>
    /// Establece el valor Alpha del sprite
    /// </summary>
    private void SetSpriteAlpha(float alpha)
    {
        if (targetSprite == null) return;

        Color newColor = targetSprite.color;
        newColor.a = alpha;
        targetSprite.color = newColor;
    }
}