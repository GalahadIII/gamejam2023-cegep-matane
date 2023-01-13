using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool _active = false;
    
    public Animator Animator;
    private static readonly int IsActivated = Animator.StringToHash("isActivated");
    private static readonly int IsReactivated = Animator.StringToHash("isReactivated");

    private void OnEnable()
    {
        Animator = GetComponent<Animator>();
    }

    public void Toggle()
    {
        _active = !_active;
        
        TriggerReset();
        if (_active) TurnOn();
        else TurnOff();
    }

    public void TurnOn()
    {
        Animator.SetTrigger(IsActivated);
    }

    public void TurnOff()
    {
        Animator.SetTrigger(IsReactivated);
    }

    public void TriggerReset()
    {
        Animator.ResetTrigger(IsActivated);
        Animator.ResetTrigger(IsReactivated);
    }
    
}
