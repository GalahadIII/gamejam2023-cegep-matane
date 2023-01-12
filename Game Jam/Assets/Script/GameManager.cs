using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    private GameManager() { Inst = this; }
    private void OnEnable()
    {
        Inst = this;
    }
    
    [SerializeField] public PlayerManager Player;
    [SerializeField] public CameraController CameraController;
    
    public int FixedUpdateCount { get; private set; } = 0;

    public Direction2D Rotation = new Direction2D(0).ChangeQuaternionAxis(Vector3.up);
    public TowerContext TowerSide { get; private set; } = TowerContext.South;

    public void Update()
    {
        // Debug.Log($"{Rotation.Quaternion.eulerAngles} {Rotation.Axis}");
        // Debug.Log($"{Rotation.Add(90).Quaternion.eulerAngles} {Rotation.Add(90).Axis}");
    }

    public void FixedUpdate()
    {
        FixedUpdateCount++;
        
        // 

        // Transform pT = Player.transform;
        // Vector3 pos = pT.position;
        // pos.z = -3;
        // pT.position = ConvertVector(pos);
    }

    public Vector3 ConvertVector(Vector3 inputVector)
    {
        // Debug.Log($"{Rotation.Quaternion.eulerAngles} {Rotation.Quaternion * inputVector} {inputVector}");
        return Rotation.Quaternion * inputVector;
        Debug.Log($"VAR");
        return ConvertVector(inputVector, TowerSide);
    }
    public Vector3 ConvertVector(Vector2 inputVector)
    {
        return Rotation.Quaternion * inputVector;
        return ConvertVector(inputVector, TowerSide);
    }
    public static Vector3 ConvertVector(Vector3 inputVector3, TowerContext towerSide)
    {
        return new Direction2D(Direction2D.ToAngle(towerSide)).ChangeQuaternionAxis(Vector3.up).Quaternion * inputVector3;
        float x = inputVector3.x;
        float y = inputVector3.y;
        float z = inputVector3.z;
        
        return towerSide switch
        {
            TowerContext.North => new Vector3(-x, y, z),
            TowerContext.South => new Vector3(x, y, -z),
            TowerContext.East => new Vector3(z, y, x),
            TowerContext.West => new Vector3(-z, y, -x),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void TurnRight()
    {
        // Debug.Log("TurnRight");
        if ((int)++TowerSide > 3) TowerSide = TowerContext.South;
        Rotation = Rotation.Add(-90);
        DoRotate();
        // CameraController.SetQuaternion(Rotation.Quaternion);
        
        // {
        //     TowerContext.South => TowerContext.East,
        //     TowerContext.East => TowerContext.North,
        //     TowerContext.North => TowerContext.West,
        //     TowerContext.West => TowerContext.South,
        //     _ => throw new ArgumentOutOfRangeException()
        // };
    }
    public void TurnLeft()
    {
        // Debug.Log("TurnLeft");
        if ((int)--TowerSide < 0) TowerSide = TowerContext.West;
        Rotation = Rotation.Add(90);
        DoRotate();

        // TowerSide = TowerSide switch
        // {
        //     TowerContext.South => TowerContext.West,
        //     TowerContext.West => TowerContext.North,
        //     TowerContext.North => TowerContext.East,
        //     TowerContext.East => TowerContext.South,
        //     _ => throw new ArgumentOutOfRangeException()
        // };
    }

    private void DoRotate()
    {
        transform.rotation = Rotation.Quaternion;
        CameraController.SetQuaternion(Rotation.Quaternion);
        Player.ResetVel();
        Transform pT = Player.transform;
        pT.rotation = Rotation.Quaternion;
        Vector3 pos = pT.position;
        pos.z = -3;
        pT.position = ConvertVector(pos);
    }
    
}

public enum TowerContext {South, East, North, West}
