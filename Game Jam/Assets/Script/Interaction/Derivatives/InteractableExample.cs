using UnityEngine;

public class InteractableExample : InteractBase, IInteractable
{
    public new Vector3 WorldPosition => transform.position + InteractionHintOffset;
    public new void ShowHint()
    {
        Debug.Log($"HINT");
    }

    public new void Interact()
    {
        Debug.Log($"INTERACT");
    }
}
