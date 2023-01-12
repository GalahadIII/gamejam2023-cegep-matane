using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Direction2D
{
    public float Angle { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector3 Axis { get; private set; } = Vector3.forward;
    public Quaternion Quaternion { get; private set; }

    public Direction2D(float angle)
    {
        Angle = angle;
        
        Quaternion = Quaternion.AngleAxis(Angle, Axis);
        
        Direction = Quaternion.AngleAxis(Angle, Axis) * Vector3.right;
    }

    public Direction2D(Vector2 direction)
    {
        Direction = direction.normalized;
        
        Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        
        Quaternion = Quaternion.AngleAxis(Angle, Axis);
    }
    
    public Direction2D(Quaternion quaternion)
    {
        Quaternion = quaternion;

        Direction = quaternion * Vector3.right;
        
        Angle = Quaternion.eulerAngles.z;
    }

    public Direction2D ChangeQuaternionAxis(Vector3 axis)
    {
        Axis = axis;
        Quaternion = Quaternion.AngleAxis(Angle, Axis);
        return this;
    }

    public Direction2D SetAngle(float angle)
    {
        Angle = angle;
        while (Angle is < 0 or > 360)
        {
            if (Angle > 360) Angle -= 360;
            if (Angle < 0) Angle += 360;
        }

        Quaternion = Quaternion.AngleAxis(Angle, Axis);
        return this;
    }

    public Direction2D Add(float angle)
    {
        return SetAngle(Angle + angle);
    }

    public Direction2D SpreadRandomized(float spreadRange)
    {
        float offset = spreadRange / 2;
        float random = Random.Range(0f, spreadRange);
        float diff = -offset + random;

        return Add(diff);
    }

    public static float ToAngle(TowerContext towerCtx)
    {
        return towerCtx switch
        {
            TowerContext.South => 0,
            TowerContext.West => 90,
            TowerContext.North => 180,
            TowerContext.East => -90,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        return $"({Angle}, {Direction}, {Quaternion})";
    }

}