using UnityEngine;

public class PlayerPrefsDebugTool : MonoBehaviour
{
    [Header("Clave para probar")]
    public string key = "Sol";
    public bool valueToSet = true;

    [ContextMenu("Mostrar valor actual")]
    public void MostrarValor()
    {
        int val = PlayerPrefs.GetInt(key, -1);
        Debug.Log($"Valor de '{key}' = {val}");
    }

    [ContextMenu("Asignar valor")]
    public void AsignarValor()
    {
        PlayerPrefs.SetInt(key, valueToSet ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"Asignado {valueToSet} a '{key}'");
    }

    [ContextMenu("Borrar clave")]
    public void BorrarClave()
    {
        PlayerPrefs.DeleteKey(key);
        Debug.Log($"Clave '{key}' borrada");
    }

    [ContextMenu("Borrar TODO PlayerPrefs")]
    public void BorrarTodo()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los PlayerPrefs borrados");
    }
}