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
    
    [SerializeField] private float _offsetOutwards = 0.5f;
    [SerializeField] private float _offsetWall = 0.51f;

    // private int _cooldown = 0;
    private bool _triggered = false;


    private void OnEnable()
    {
        float offsetOut = GameManager.Inst.TowerSize / 2 + _offsetOutwards;
        float offsetWall = GameManager.Inst.TowerSize / 2 + _offsetWall;
         
        SwitcherLeft.transform.localPosition = new Vector3(-offsetOut, 0, -offsetWall);
        SwitcherRight.transform.localPosition = new Vector3(offsetOut, 0, -offsetWall);

        _wallLeft.transform.localPosition = new Vector3(-offsetOut-1, 0, -offsetWall);
        _wallRight.transform.localPosition = new Vector3(offsetOut+1, 0, -offsetWall);
    }

    private void FixedUpdate()
    {
        Transform t = transform;
        {
            Transform pT = _playerFollowY.transform;
            float offsetWall = GameManager.Inst.TowerSize / 2 + _offsetWall;
            Vector3 posLock = pT.position;
            TowerContext towerContext = GameManager.Inst.TowerSide;
            
            if (towerContext == TowerContext.East) posLock.x = offsetWall;
            else if (towerContext == TowerContext.West) posLock.x = -offsetWall;
            else if (towerContext == TowerContext.North) posLock.z = offsetWall;
            else if (towerContext == TowerContext.South) posLock.z = -offsetWall;
            Debug.Log($"{posLock}");
            pT.position = posLock;
            

            t.position = new Vector3(0, pT.position.y, 0);
        }
            
        t.rotation = GameManager.Inst.Rotation.Quaternion;
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
