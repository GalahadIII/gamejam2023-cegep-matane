using UnityEngine;

public enum PlaneSwitchDirection{Left, Right}

public class PlaneSwitcherInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _playerFollowY = null;
    [SerializeField] private PlaneSwitchDirection _planeSwitchDirection = PlaneSwitchDirection.Right;

    private void FixedUpdate()
    {
        if (_playerFollowY == null) return;

        Transform t = transform;
        Vector3 pos = t.position;
        pos.y = _playerFollowY.transform.position.y;
        t.position = pos;

    }

    public void ShowHint()
    {
        
    }

    public void Interact()
    {
        GameManager.Inst.ChangePlane(_planeSwitchDirection);
    }
}
