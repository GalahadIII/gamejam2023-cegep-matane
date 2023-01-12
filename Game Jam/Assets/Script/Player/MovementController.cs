using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private PlayerMoveStats _stats;
    [SerializeField] private LayerMask _groundLayerMask;
    
    private Rigidbody _rb;
    private PlayerInputs _frameInput;
    private InteractionModule _interactionModule;

    private int _fixedUpdateCounter;
    private bool _hasControl;
    private bool _grounded;
    private bool _facingRight;
    private float _gravityScale;
    
    #region Public
    
    public Vector2 Speed => _rb.velocity;
    public bool Falling => _rb.velocity.y < 0;
    public bool Jumping => _rb.velocity.y > 0;
    public bool FacingRight => _facingRight;
    public bool Grounded => _grounded;
    
    #endregion
    
    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _frameInput = InputManager.PlayerInputs;

        _grounded = true;
        
        if (_frameInput.Jump.OnDown)
        {
            _jumpToConsume = true;
            _frameJumpWasPressed = _fixedUpdateCounter;
        }

        if (_frameInput.Interact.OnDown)
        {
            _interactionModule.TriggerInteraction();
        }
    }
    
    private void FixedUpdate()
    {
        _fixedUpdateCounter++;

        // input dependant
        //Flip();
        Horizontal();
        Jump();
        //ArtificialFriction();
        
        // !input dependant
        GravityModifier();
    }

    #region Horizontal

    private void Horizontal()
    {
        // calculate wanted direction and desired velocity
        float targetSpeed = (_frameInput.Movement2d.Live.x == 0 ? 0 : MathF.Sign(_frameInput.Movement2d.Live.x))  * _stats.MoveSpeed;
        // calculate difference between current volocity and target velocity
        float speedDif = targetSpeed - _rb.velocity.x;
        // change acceleration rate depending on situations;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Decceleration;
        // applies acceleration to speed difference, raise to a set power so acceleration increase with higher speed
        // multiply by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);

        // apply the movement force
        _rb.AddForce(movement * GameManager.Inst.ConvertVector(Vector2.right));
    }
    #endregion
    
    #region Jump

     private bool _jumpToConsume;
     private int _frameJumpWasPressed;
     private int _frameLeftGround;

     private bool _bufferedJumpUsable;
     private bool _coyoteUsable;
     
     
     private bool HasBufferedJump =>
         _bufferedJumpUsable && _fixedUpdateCounter < _frameJumpWasPressed + _stats.JumpBufferFrame;
     private bool CanUseCoyote => 
         _coyoteUsable && !_grounded && _fixedUpdateCounter < _frameLeftGround + _stats.JumpCoyoteFrame;

     private void Jump()
     {
         // if (_jumpToConsume || HasBufferedJump)
         // {
         //     if (_grounded || CanUseCoyote) NormalJump();
         // }
         // _jumpToConsume = false;
         if (_jumpToConsume)
         {
             NormalJump();
         }
         _jumpToConsume = false;
     }

     private void NormalJump()
     {
         _coyoteUsable = false;
         _bufferedJumpUsable = false;
         _rb.velocity = new Vector2(_rb.velocity.x, 0);
         _rb.AddForce(Vector2.up * _stats.JumpForce, ForceMode.Impulse);
     }

     private void ResetJump()
     {
         _coyoteUsable = true;
         _bufferedJumpUsable = true;
     }

    private void GravityModifier()
    {
        if (!Falling) return;
        _rb.AddForce(Vector3.down * _stats.FallGravityForce, ForceMode.Force);
    }
    #endregion
}
