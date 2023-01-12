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
    
    public PlayerManager Player;
    public CameraController CameraController;
    
    public int FixedUpdateCount { get; private set; } = 0;

    public Direction2D Rotation = new Direction2D(0).ChangeQuaternionAxis(Vector3.up);
    public float TowerSize = 5;
    public TowerContext TowerSide { get; private set; } = TowerContext.South;
    public float PlayerTowerDistance = 3.5f;

    public void FixedUpdate()
    {
        FixedUpdateCount++;
    }

    public Vector3 ConvertVector(Vector3 inputVector)
    {
        return ConvertVector(inputVector, TowerSide);
    }
    public Vector3 ConvertVector(Vector2 inputVector)
    {
        return ConvertVector(inputVector, TowerSide);
    }
    public static Vector3 ConvertVector(Vector3 inputVector3, TowerContext towerSide)
    {
        // Vector3 transformedVector = towerSide switch
        // {
        //     TowerContext.North => new Vector3(-inputVector3.x, inputVector3.y, inputVector3.z),
        //     TowerContext.South => new Vector3(inputVector3.x, inputVector3.y, -inputVector3.z),
        //     TowerContext.East => new Vector3(inputVector3.z, inputVector3.y, inputVector3.x),
        //     TowerContext.West => new Vector3(-inputVector3.z, inputVector3.y, -inputVector3.x),
        //     _ => throw new ArgumentOutOfRangeException()
        // };
        
        Direction2D dir = new Direction2D(Direction2D.ToAngle(towerSide)).ChangeQuaternionAxis(Vector3.up);
        Vector3 pivotedVector = dir.Quaternion * inputVector3;
        
        // Debug.Log($"{transformedVector} {pivotedVector}");
        
        return pivotedVector;
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
        Debug.Log($"{Rotation.Quaternion.eulerAngles}");
        
        transform.rotation = Rotation.Quaternion;
        
        CameraController.SetQuaternion(Rotation.Quaternion);
        
        Player.SetQuaternion(Rotation.Quaternion, CameraController.rotationSpeed);
        Player.ResetVel();
        Player.FreezePosition(TowerContextToFreezePosition(TowerSide));
    }

    private static FreezePositionAxis TowerContextToFreezePosition(TowerContext towerContext)
    {
        // if (towerContext == TowerContext.East || towerContext == TowerContext.West)
            // FreezePositionAxis
        
        return towerContext switch
        {
            TowerContext.South => FreezePositionAxis.Z,
            TowerContext.West => FreezePositionAxis.X,
            TowerContext.North => FreezePositionAxis.Z,
            TowerContext.East => FreezePositionAxis.X,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    
    
}

public enum TowerContext {South, East, North, West}
