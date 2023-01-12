using UnityEngine;

public class PlaneSwitcherFollow : MonoBehaviour
{

    [SerializeField] private GameObject Left;
    [SerializeField] private GameObject Right;
    
    [SerializeField] private GameObject _playerFollowY = null;
    [SerializeField] private float _towerSize = 5;
    [SerializeField] private float _offsetOutwards = 1.2f;

    private void OnEnable()
    {
        float offset = _towerSize / 2 + _offsetOutwards;
        Left.transform.localPosition = GameManager.Inst.ConvertVector(new Vector3(-offset, 0, 0));
        Right.transform.localPosition = GameManager.Inst.ConvertVector(new Vector3(offset, 0, 0));
    }

    private void FixedUpdate()
    {
        if (_playerFollowY == null) return;

        Transform t = transform;
        Vector3 pos = t.position;
        pos.y = _playerFollowY.transform.position.y;
        t.position = pos;

    }
}
