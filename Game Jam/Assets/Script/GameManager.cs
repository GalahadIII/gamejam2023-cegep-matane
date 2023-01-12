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

    public int FixedUpdateCount { get; private set; } = 0;
    public void FixedUpdate()
    {
        FixedUpdateCount++;
    }

    public TowerContext TowerSide { get; private set; } = TowerContext.South;

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

    public void ChangePlane(PlaneSwitchDirection planeSwitchDirection)
    {
        if (planeSwitchDirection == PlaneSwitchDirection.Left) TurnLeft();
        else if (planeSwitchDirection == PlaneSwitchDirection.Right) TurnRight();
    }
    private void TurnRight()
    {
        Debug.Log("TurnRight");
        TowerSide = TowerSide switch
        {
            TowerContext.South => TowerContext.East,
            TowerContext.East => TowerContext.North,
            TowerContext.North => TowerContext.West,
            TowerContext.West => TowerContext.South,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    private void TurnLeft()
    {
        Debug.Log("TurnLeft");
        TowerSide = TowerSide switch
        {
            TowerContext.South => TowerContext.West,
            TowerContext.West => TowerContext.North,
            TowerContext.North => TowerContext.East,
            TowerContext.East => TowerContext.South,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [SerializeField] private GameObject Player;
    
}

public enum TowerContext {North, South, East, West}
