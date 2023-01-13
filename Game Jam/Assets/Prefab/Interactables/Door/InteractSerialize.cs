using UnityEngine;
using UnityEngine.Events;

public class InteractSerialize : MonoBehaviour, IInteractable
{
    public UnityEvent InteractHint;
    public UnityEvent InteractTrigger;

    public void ShowHint()
    {
        InteractHint.Invoke();
    }
    public void Interact()
    {
        InteractTrigger.Invoke();
    }
}
