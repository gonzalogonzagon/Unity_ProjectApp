using UnityEngine;

public class SceneContentManager : MonoBehaviour
{
    [SerializeField] private GameObject currentFocusObject;
    [SerializeField] private GameObject nextFocusObject;

    [SerializeField] private TouchManager touchManager; // Asume que tienes un TouchManager que maneja el contenido enfocado

    public void Focus()
    {

    }
}
