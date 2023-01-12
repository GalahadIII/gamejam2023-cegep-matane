using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    private void OnEnable()
    {
        Inst = this;
    }

    public TowerContext TowerSide { get; private set; } = TowerContext.South;
    public Vector3 ConvertVector2(Vector2 inputVector2)
    {
        float x = inputVector2.x;
        float y = inputVector2.y;
        
        return TowerSide switch
        {
            TowerContext.North => new Vector3(-x, y, 0),
            TowerContext.South => new Vector3(x, y, 0),
            TowerContext.East => new Vector3(0, y, x),
            TowerContext.West => new Vector3(0, y, -x),
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
        TowerSide = TowerSide switch
        {
            TowerContext.South => TowerContext.West,
            TowerContext.West => TowerContext.North,
            TowerContext.North => TowerContext.East,
            TowerContext.East => TowerContext.South,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum TowerContext {North, South, East, West}
