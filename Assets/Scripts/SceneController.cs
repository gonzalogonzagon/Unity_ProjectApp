using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    // Singleton pattern para fácil acesso a este script
    public static SceneManagement Instance {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para ser llamado desde Android
    public void LoadScene(string sceneName)
    {
        Debug.Log("Cargando escena: " + sceneName);
        SceneManager.LoadScene(sceneName);
    } 

    public void LoadSceneByIndex(int sceneIndex)
    {
        Debug.Log("Cargando escena índice: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}
