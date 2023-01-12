using UnityEngine;

public class PlaneSwitcherFollow : MonoBehaviour
{
    [SerializeField] private GameObject _playerFollowY = null;
    private void FixedUpdate()
    {
        if (_playerFollowY == null) return;

        Transform t = transform;
        Vector3 pos = t.position;
        pos.y = _playerFollowY.transform.position.y;
        t.position = pos;

    }
}
