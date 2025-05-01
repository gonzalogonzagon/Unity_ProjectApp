using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private bool flipFace = false;
    [SerializeField] private bool useWorldUp = false; // Para mantener "arriba" alineado con el mundo

    void Start()
    {
        FindMainCamera();
    }

    void FindMainCamera()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Usar LateUpdate para asegurar que se ejecute después del movimiento de la cámara
    void LateUpdate()
    {
        if (cameraTransform == null)
        {
            FindMainCamera();
            if (cameraTransform == null) return;
        }

        if (useWorldUp)
        {
            // Mantiene el objeto vertical (útil para sprites/texto)
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            directionToCamera.y = 0; // Ignorar diferencia de altura para mantener vertical
            
            if (directionToCamera != Vector3.zero) // Evitar advertencias
            {
                transform.rotation = Quaternion.LookRotation(directionToCamera);
            }
        }
        else
        {
            // Billboard completo (el objeto se orienta completamente hacia la cámara)
            transform.LookAt(cameraTransform.position);
        }

        if (flipFace)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}