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
        float hor = inputVector2.x;
        float ver = inputVector2.y;
        
        return TowerSide switch
        {
            TowerContext.North => new Vector3(-hor, ver, 0),
            TowerContext.South => new Vector3(hor, ver, 0),
            TowerContext.East => new Vector3(0, ver, hor),
            TowerContext.West => new Vector3(0, ver, -hor),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public void TurnRight()
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
    public void TurnLeft()
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