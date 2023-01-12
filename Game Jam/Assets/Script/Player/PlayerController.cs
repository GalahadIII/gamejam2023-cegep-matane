using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] private PlayerMoveStats _stats;

    #region Private

    [SerializeField] private LayerMask _groundLayerMask; 
    
    private Rigidbody _rb;
    private PlayerInputs _input;

    private int _fixedUpdateCounter;
    private bool _hasControl;
    private bool _grounded;
    private bool _facingRight;
    private float _gravityScale;
    private BoxCollider2D _feetCollider;
    private CapsuleCollider2D _headCollider;

    #endregion

    #region Public
    
    public bool Falling => _rb.velocity.y < 0;
    public bool Jumping => _rb.velocity.y > 0;
    public bool FacingRight => _facingRight;
    public bool Grounded => _grounded;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        //IsGrounded();
                    
        _input = InputManager.PlayerInputs;
        if (_input.Jump.OnDown)
        {
            _jumpToConsume = true;
            _frameJumpWasPressed = _fixedUpdateCounter;
        }
        if (_input.Jump.OnUp) JumpCut();
    }
    
    private void FixedUpdate()
    {
        _fixedUpdateCounter++;

        // input dependant
        Flip();
        Horizontal();
        Jump();
        ArtificialFriction();
        
        // !input dependant
        GravityModifier();
    }

    /*#region Checks

    private void IsGrounded()
    {
        float extraHeightText = 0.030f;
        Bounds bounds = _feetCollider.bounds;
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, extraHeightText,
            _groundLayerMask);

        if (!raycastHit && _grounded)
        {
            _frameLeftGround = _fixedUpdateCounter;
        }
        else if (raycastHit && !_grounded)
        {
            ResetJump();
        }
        
        _grounded = raycastHit;
    }

    #endregion*/

    #region Horizontal

    private void Horizontal()
    {
        // calculate wanted direction and desired velocity
        float targetSpeed = (_input.Movement2d.Live.x == 0 ? 0 : MathF.Sign(_input.Movement2d.Live.x))  * _stats.MoveSpeed;
        // calculate difference between current volocity and target velocity
        float speedDif = targetSpeed - _rb.velocity.x;
        // change acceleration rate depending on situations;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Decceleration;
        // applies acceleration to speed difference, raise to a set power so acceleration increase with higher speed
        // multiply by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);

        // apply the movement forcea 
        _rb.AddForce(movement * Vector3.right);
    }

    private void ArtificialFriction()
    {
        if (!_grounded) return;
        if (!(Mathf.Abs(_input.Movement2d.Live.x) < 0.01f)) return;
        
        // use either friction amount or velocity
        float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_stats.Friction));
        // sets to movement direction
        amount *= Mathf.Sign(_rb.velocity.x);
        // applies force against movement direction
        _rb.AddForce(Vector3.right * -amount, ForceMode.Impulse);

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
        _rb.velocity = new Vector3(_rb.velocity.x, 0);
        _rb.AddForce(Vector3.up * _stats.JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _coyoteUsable = true;
        _bufferedJumpUsable = true;
    }

    private void JumpCut()
    {
        if (!Falling)
        {
            _rb.AddForce(Vector3.down * (_rb.velocity.y * (1 - _stats.JumpCutMultiplier)), ForceMode.Impulse);
        }
    }

    private void GravityModifier()
    {
        //_rb.gravityScale = Falling ? _stats.FallGravityMultiplier : _gravityScale;
    }

    #endregion

    private void Flip()
    {
        if (Mathf.Abs(_input.Movement2d.Live.x) < 0.1f) return;
        if (_facingRight && Mathf.Sign(_input.Movement2d.Live.x) < 0) return;
        if (!_facingRight && Mathf.Sign(_input.Movement2d.Live.x) > 0) return;
        //Utilities.FlipTransform(transform);
        _facingRight = !_facingRight;
    }
}

public interface IPlayerController
{
    public bool Falling { get; }
    public bool Jumping { get; }
    public bool FacingRight { get; }
    public bool Grounded { get; }
}

