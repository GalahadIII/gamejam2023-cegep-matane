using UnityEngine;


[CreateAssetMenu]
public class PlayerMoveStats : ScriptableObject
{
    [Header("MOVEMENT")]
    public float MoveSpeed = 20f;
    
    public float Acceleration = 6f;
    
    public float Decceleration = 6f;

    public float VelPower = 1.2f;

    public float Friction = 0.7f;
    
    
    [Header("JUMP")]
    public float JumpForce = 5f;
    
    // reduces current y velocity by amount[0-1] (higher the CutMultiplier the less sensitive to input it becomes)
    public float JumpCutMultiplier = 0.5f;
    
    public int JumpCoyoteFrame = 10;
    
    public int JumpBufferFrame = 10;

    public float FallGravityMultiplier = 1.9f;


    [Header("DASH")] 
    public bool AllowDash = false;
    
    public float DashVelocity = 50;

    public int DashDurationFrame = 10;

    [Header("ROLL")] 
    public bool AllowRoll = false;

    public float RollVelocity = 30;

    public int RollDurationFrame = 10;


}


