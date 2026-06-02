using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Interactable;

public class Destroy : MonoBehaviour, IInteractable
{
    public string InteractMessage => objectInteractMessage;
    public UnityEvent onInteract;

    [SerializeField] string objectInteractMessage;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            onInteract.Invoke();
        }
    }

    public void Interact()
    {
        //throw new System.NotImplementedException();

    }
}
