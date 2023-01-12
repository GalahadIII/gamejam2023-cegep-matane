using System.Linq;
using UnityEngine;

public enum PlaneSwitchDirection{Left, Right}

public class PlaneSwitcher : ColliderDetector
{
    [SerializeField] private PlaneSwitchDirection _planeSwitchDirection = PlaneSwitchDirection.Right;

    private bool _triggered = false;
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (_triggered && Objects.Count < 1) _triggered = false;
        
        if (_triggered) return;

        foreach (GameObject o in Objects.Where(o => o.CompareTag("Player")))
        {
            GameManager.Inst.ChangePlane(_planeSwitchDirection);
            _triggered = true;
        }
    }
}