using UnityEngine;

public class ButtonExitController : MonoBehaviour
{
    public void PrintExitMessage()
    {
        Debug.Log("Saliendo de la aplicación...");
    }   

/// <summary>
    /// Finaliza la actividad de Unity y vuelve a la actividad anterior de Android
    /// </summary>
    public void ReturnToAndroidApp()
    {
        Debug.Log("Intentando volver a la aplicación Android...");
        
        #if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        activity.Call("finish");
                        // Alternativamente, si necesitas pasar datos a la actividad Android:
                        // activity.Call("returnToMainApp", "datos_adicionales");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al comunicar con Android: " + e.Message);
            }
        #else
            Debug.Log("Función simulada: Volviendo a la app Android (solo funcionará en dispositivo real)");
            // Para pruebas en editor, podrías añadir alguna acción alternativa
        #endif
    }
}
