using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Interactable;

public class InteractionController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactionDistance = 3f;

    IInteractable currentTargetInteractable;

    public void Update()
    {
        UpdateCurrentTargetInteractable();

        UpdateInteractionText();

        CheckForInteractionInput();
    }

    private void UpdateCurrentTargetInteractable()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out var hit, interactionDistance);
        currentTargetInteractable = hit.collider?.GetComponent<IInteractable>();
    }

    void UpdateInteractionText()
    {
        if (currentTargetInteractable == null)
        {
            interactionText.text = string.Empty;
            return;
        }

        interactionText.text = currentTargetInteractable.InteractMessage;

    }

    void CheckForInteractionInput()
    {
        if(Input.GetKeyDown(KeyCode.E) && currentTargetInteractable != null)
        {
            currentTargetInteractable.Interact();
        }
    }

}
