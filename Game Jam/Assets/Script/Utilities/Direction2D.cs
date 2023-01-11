using UnityEngine;

public class Direction2D
{
    public float Angle { get; }
    public Vector2 Direction { get; }
    public Quaternion Quaternion { get; private set; }

    public Direction2D(float angle)
    {
        Angle = angle;
        
        Quaternion = Quaternion.AngleAxis(Angle, Vector3.forward);
        
        Direction = Quaternion.AngleAxis(Angle, Vector3.forward) * Vector3.right;
    }

    public Direction2D(Vector2 direction)
    {
        Direction = direction.normalized;
        
        Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        
        Quaternion = Quaternion.AngleAxis(Angle, Vector3.forward);
    }
    
    public Direction2D(Quaternion quaternion)
    {
        Quaternion = quaternion;

        Direction = quaternion * Vector3.right;
        
        Angle = Quaternion.eulerAngles.z;
    }

    public Direction2D ChangeQuaternionAxis(Vector3 axis)
    {
        Quaternion = Quaternion.AngleAxis(Angle, axis);
        return this;
    }

    public Direction2D Add(float angle)
    {
        return new Direction2D(Angle + angle);
    }

    public Direction2D SpreadRandomized(float spreadRange)
    {
        float offset = spreadRange / 2;
        float random = Random.Range(0f, spreadRange);
        float diff = -offset + random;

        return Add(diff);
    }

    public override string ToString()
    {
        return $"({Angle}, {Direction}, {Quaternion})";
    }

}