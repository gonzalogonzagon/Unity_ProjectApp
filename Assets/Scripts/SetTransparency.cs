using UnityEngine;

public class SetTransparency : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer targetSprite;

    public void SetSpriteAlpha() {
        if (targetSprite != null) {
            Color c = targetSprite.color;
            if (c.a == 1f) {
                c.a = 0.5f;
            } else {
                c.a = 1f;
            }
            targetSprite.color = c;
        }
    }

    public void Interact()
    {
        SetSpriteAlpha();
    }

    public bool CanInteract()
    {
        return true;
    }
}