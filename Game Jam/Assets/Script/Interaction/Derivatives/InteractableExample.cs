using UnityEngine;

public class InteractableExample : MonoBehaviour, IInteractable
{
    public void ShowHint()
    {
        Debug.Log($"HINT");
    }

    public void Interact()
    {
        Debug.Log($"INTERACT");
    }
}
