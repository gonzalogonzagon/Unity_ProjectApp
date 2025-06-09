using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition3DButton : MonoBehaviour, IInteractable
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 newCameraPosition;
    [SerializeField] private Transform lookAtTarget;

    [Header("Marcadores InfoM")]
    [SerializeField] private GameObject infoMCloseDoor;
    [SerializeField] private GameObject infoMOpenDoor;
    [SerializeField] private GameObject infoMOpenDoor2;
    [SerializeField] private Animator doorAnimator;

    [Header("Botones a habilitar")]
    [SerializeField] private List<GameObject> buttonsToEnable;
    [Header("Botones a deshabilitar")]
    [SerializeField] private List<GameObject> buttonsToDisable;

    public void Interact()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = newCameraPosition;
            if (lookAtTarget != null)
            {
                mainCamera.transform.LookAt(lookAtTarget);
            }
        }

        // Habilita los botones deseados
        foreach (var btn in buttonsToEnable)
        {
            if (btn != null) btn.SetActive(true);
        }
        // Deshabilita los demás
        foreach (var btn in buttonsToDisable)
        {
            if (btn != null) btn.SetActive(false);
        }

        // Mostrar/ocultar marcadores InfoM según el estado de la puerta
        if (doorAnimator != null)
        {
            bool isOpen = doorAnimator.GetBool("IsOpen");
            if (infoMOpenDoor != null) { 
                infoMOpenDoor.SetActive(isOpen);
                infoMOpenDoor2.SetActive(isOpen);
            }
            if (infoMCloseDoor != null) infoMCloseDoor.SetActive(!isOpen);                
        }
    }

    public bool CanInteract()
    {
        // Puedes añadir lógica adicional si lo necesitas
        return true;
    }
}
