using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaneSwitcherFollow : MonoBehaviour
{

    [SerializeField] public ColliderDetector SwitcherLeft;
    [SerializeField] public ColliderDetector SwitcherRight;

    [SerializeField] private GameObject _wallLeft;
    [SerializeField] private GameObject _wallRight;
    
    [SerializeField] private GameObject _playerFollowY = null;
    
    [SerializeField] private float _offsetOutwards = 0.7f;

    // private int _cooldown = 0;
    private bool _triggered = false;

    private void OnEnable()
    {
        float offset = GameManager.Inst.TowerSize / 2 + _offsetOutwards;

        SwitcherLeft.transform.localPosition = new Vector3(-offset, 0, -offset);
        SwitcherRight.transform.localPosition = new Vector3(offset, 0, -offset);

        _wallLeft.transform.localPosition = new Vector3(-offset-1, 0, -offset);
        _wallRight.transform.localPosition = new Vector3(offset+1, 0, -offset);
    }

    private void FixedUpdate()
    {
        // _cooldown++;
        
        if (_playerFollowY != null)
        {
            Vector3 pos = Vector3.zero;
            pos.y = _playerFollowY.transform.position.y;
            transform.position = pos;
        }
            
        transform.rotation = GameManager.Inst.Rotation.Quaternion;
            // transform.rotation = new Direction2D(_playerFollowY.transform.rotation).ChangeQuaternionAxis(Vector3.up).Quaternion;
        // if (_cooldown < 50) return;

        List<GameObject> obj = new();
        obj.AddRange(SwitcherLeft.Objects);
        obj.AddRange(SwitcherRight.Objects);
        
        if (_triggered && obj.Count < 1) _triggered = false;
        
        if (_triggered) return;
        
        foreach (GameObject o in SwitcherLeft.Objects.Where(o => o.CompareTag("Player")))
        {
            PlayerDetected(o, SwitcherLeft);
            GameManager.Inst.TurnLeft();
            return;
        }
        foreach (GameObject o in SwitcherRight.Objects.Where(o => o.CompareTag("Player")))
        {
            PlayerDetected(o, SwitcherRight);
            GameManager.Inst.TurnRight();
            return;
        }
    }

    private void PlayerDetected(GameObject player, Component detector)
    {
        player.transform.position = detector.transform.position;
        // _cooldown = 0;
        _triggered = true;
    }
}
