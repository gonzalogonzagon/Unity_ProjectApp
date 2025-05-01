using UnityEngine;

public class SafeAreaAdjuster : MonoBehaviour
{
    [SerializeField] private RectTransform safeAreaContainer;
    private Rect safeAreaRect;
    Vector2 minAnchor;
    Vector2 maxAnchor;
    
    private void Awake()
    {
        // Find the SafeAreaContainer if not assigned in inspector
        if (safeAreaContainer == null)
        {
            Transform child = transform.Find("SafeAreaContainer");
            if (child != null)
            {
                safeAreaContainer = child.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("SafeAreaContainer not found as a child of this GameObject!");
                return;
            }
        }
        
        // Get the safe area rect
        safeAreaRect = Screen.safeArea;
        
        // Calculate the anchors as a proportion of the screen size
        minAnchor = safeAreaRect.position;
        maxAnchor = minAnchor + safeAreaRect.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        // Apply the anchors to the SafeAreaContainer's RectTransform
        safeAreaContainer.anchorMin = minAnchor;
        safeAreaContainer.anchorMax = maxAnchor;
        
        // Reset the container's position since the anchors will position it correctly
        safeAreaContainer.offsetMin = Vector2.zero;
        safeAreaContainer.offsetMax = Vector2.zero;
    }
}