using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlaneSwitchDirection{Left, Right}
public class PlaneSwitcherFollow : MonoBehaviour
{

    [SerializeField] private ColliderDetector _switcherLeft;
    [SerializeField] private ColliderDetector _switcherRight;
    
    [SerializeField] private GameObject _playerFollowY = null;
    
    [SerializeField] private float _towerSize = 5;
    [SerializeField] private float _offsetOutwards = 0.7f;

    private int _cooldown = 0;
    private bool _triggered = false;

    private void OnEnable()
    {
        float offset = _towerSize / 2 + _offsetOutwards;

        _switcherLeft.transform.localPosition = GameManager.Inst.ConvertVector(new Vector3(-offset, 0, -offset));
        _switcherRight.transform.localPosition = GameManager.Inst.ConvertVector(new Vector3(offset, 0, -offset));
    }

    private void FixedUpdate()
    {
        _cooldown++;
        
        if (_playerFollowY != null)
        {
            Vector3 pos = Vector3.zero;
            pos.y = _playerFollowY.transform.position.y;
            transform.position = pos;
        }
            
        transform.rotation = GameManager.Inst.Rotation.Quaternion;
            // transform.rotation = new Direction2D(_playerFollowY.transform.rotation).ChangeQuaternionAxis(Vector3.up).Quaternion;
        if (_cooldown < 50) return;

        List<GameObject> obj = new();
        obj.AddRange(_switcherLeft.Objects);
        obj.AddRange(_switcherRight.Objects);
        
        if (_triggered && obj.Count < 1) _triggered = false;
        
        if (_triggered) return;
        
        foreach (GameObject o in _switcherLeft.Objects.Where(o => o.CompareTag("Player")))
        {
            _cooldown = 0;
            GameManager.Inst.TurnLeft();
            _triggered = true;
            return;
        }
        foreach (GameObject o in _switcherRight.Objects.Where(o => o.CompareTag("Player")))
        {
            _cooldown = 0;
            GameManager.Inst.TurnRight();
            _triggered = true;
            return;
        }

    }
}
