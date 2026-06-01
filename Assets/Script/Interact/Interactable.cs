using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public interface IInteractable
    {
        public string InteractMessage { get; }
        public void Interact();
    }

}
