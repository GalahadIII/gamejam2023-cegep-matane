using System.Linq;
using UnityEngine;

public enum PlaneSwitchDirection{Left, Right}

public class PlaneSwitcher : ColliderDetector
{
    [SerializeField] private PlaneSwitchDirection _planeSwitchDirection = PlaneSwitchDirection.Right;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (GameObject o in Objects.Where(o => o.CompareTag("Player")))
            GameManager.Inst.ChangePlane(_planeSwitchDirection);
    }
}