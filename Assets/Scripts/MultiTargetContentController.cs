using UnityEngine;
using Vuforia;

public class MultiTargetContentController : MonoBehaviour
{
    [SerializeField]
    public GameObject content;

    [SerializeField]
    [Tooltip("Lista de ImageTargetBehaviour que representan los targets a detectar")]
    public ImageTargetBehaviour[] targets;

    private ImageTargetBehaviour currentTarget;

    void Start()
    {
        foreach (var target in targets)
        {
            target.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        content.SetActive(false);
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (currentTarget == null)
            {
                currentTarget = (ImageTargetBehaviour)behaviour;
                content.transform.SetParent(currentTarget.transform, true);
                content.SetActive(true);
            }
        }
        else if (currentTarget == behaviour)
        {
            content.SetActive(false);
            content.transform.SetParent(null, true);
            currentTarget = null;
        }
    }

    void OnDestroy()
    {
        foreach (var target in targets)
        {
            target.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}