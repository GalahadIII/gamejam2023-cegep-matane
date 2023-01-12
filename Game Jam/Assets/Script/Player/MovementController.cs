using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private PlayerMoveStats _stats;
    [SerializeField] private LayerMask _groundLayerMask;
    
    private Rigidbody _rb;
    private PlayerInputs _frameInput;
    private InteractionModule _interactionModule;

    private bool _hasControl;
    private bool _grounded;
    private bool _facingRight;
    private float _gravityScale;
    
    #region Public

    public bool DisabledControls;
    
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
        
        IsGrounded();
        // Debug.Log(_grounded);
        if (_frameInput.Jump.OnDown)
        {
            _jumpToConsume = true;
            _frameJumpWasPressed = GameManager.Inst.FixedUpdateCount;
        }
    }
    
    private void FixedUpdate()
    {
        if (DisabledControls) return;
        Horizontal();
        Jump();
        GravityModifier();
    }
    
    
    #region Checks

    private void IsGrounded()
    {
        bool groundedLive = Physics.Raycast(transform.position, Vector3.down, 1.01f, _groundLayerMask);
        if (!groundedLive && _grounded)
        {
            _frameLeftGround = GameManager.Inst.FixedUpdateCount;
        }
        else if (groundedLive && !_grounded)
        {
             ResetJump();
        }
             
        _grounded = groundedLive;
    }

    #endregion
    
    #region Horizontal

    private void Horizontal()
    {
        // calculate wanted direction and desired velocity
        float targetSpeed = (_frameInput.Movement2d.Live.x == 0 ? 0 : MathF.Sign(_frameInput.Movement2d.Live.x))  * _stats.MoveSpeed;
        // calculate difference between current volocity and target velocity
        //float speedDif = targetSpeed - GameManager.Inst.ConvertVector(_rb.velocity).x * 0.9f;
        
        
        float speedDif = targetSpeed - _rb.velocity.z * 0.9f;
        //float speedDif = targetSpeed - _rb.velocity.x * 0.9f;
        
        // speedDif = Mathf.Abs(speedDif) < 0.1f || Mathf.Abs(speedDif) > 11f ? 0 : speedDif; 
        // Debug.Log($"{speedDif}");
        // change acceleration rate depending on situations;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Decceleration;
        // applies acceleration to speed difference, raise to a set power so acceleration increase with higher speed
        // multiply by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);
        
        // apply the movement force
        // Debug.Log($"{targetSpeed} {speedDif} {accelRate} {movement} {GameManager.Inst.ConvertVector(_rb.velocity).x}");
        // Debug.Log($"{GameManager.Inst.ConvertVector(_rb.velocity)} {_rb.velocity}");
        // Debug.Log($"{movement * GameManager.Inst.ConvertVector(Vector2.right)}");
        //_rb.AddForce(movement * GameManager.Inst.ConvertVector(Vector2.right));
        
        
        //_rb.AddForce(movement * Vector3.right);
        _rb.AddForce(movement * Vector3.forward);
    }
    #endregion
    
    #region Jump

    private bool _jumpToConsume;
    private int _frameJumpWasPressed;
    private int _frameLeftGround;

    private bool _bufferedJumpUsable;
    private bool _coyoteUsable;
     
     
    private bool HasBufferedJump =>
         _bufferedJumpUsable && GameManager.Inst.FixedUpdateCount < _frameJumpWasPressed + _stats.JumpBufferFrame;
    private bool CanUseCoyote => 
         _coyoteUsable && !_grounded && GameManager.Inst.FixedUpdateCount < _frameLeftGround + _stats.JumpCoyoteFrame;

    private void Jump()
    {
        if (_jumpToConsume || HasBufferedJump)
        {
            if (_grounded || CanUseCoyote) NormalJump();
        }
        _jumpToConsume = false;
    }

    private void NormalJump()
    {
        _coyoteUsable = false;
        _bufferedJumpUsable = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.y);
        _rb.AddForce(Vector3.up * _stats.JumpForce, ForceMode.Impulse);
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

    public void ResetVelocity()
    {
        _rb.velocity = Vector3.zero;
    }
}
