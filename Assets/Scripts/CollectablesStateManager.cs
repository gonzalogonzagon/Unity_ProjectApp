using System.Collections.Generic;
using UnityEngine;

public class CollectablesStateManager : MonoBehaviour
{
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("¡Todos los PlayerPrefs han sido eliminados!");
    }
}
