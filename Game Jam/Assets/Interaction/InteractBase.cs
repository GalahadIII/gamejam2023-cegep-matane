using UnityEngine;
using UnityEngine.Events;

public class InteractBase : MonoBehaviour, IInteractable
{
    public Vector3 InteractionHintOffset = new(0, 2, 0);
    
    public UnityEvent InteractHint;
    public UnityEvent InteractTrigger;

    public Vector3 WorldPosition => transform.position + InteractionHintOffset;

    public void ShowHint()
    {
        InteractionGUI.Display();
        InteractHint.Invoke();
    }
    public void Interact()
    {
        InteractTrigger.Invoke();
    }
}
