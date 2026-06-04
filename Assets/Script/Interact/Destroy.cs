using UnityEngine;
using UnityEngine.Events;
using static Interactable;

public class Destroy : MonoBehaviour, IInteractable
{
    public string InteractMessage => objectInteractMessage;
    [SerializeField] private UnityEvent onInteract;

    [SerializeField] string objectInteractMessage;

    // ”брано глобальное прослушивание Input.GetKeyDown(KeyCode.E)
    // Ч теперь событие вызываетс€ только через Interact()

    public void Interact()
    {
        onInteract?.Invoke();
    }
}
