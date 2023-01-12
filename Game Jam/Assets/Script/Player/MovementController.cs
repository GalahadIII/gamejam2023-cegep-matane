using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private PlayerMoveStats _stats;
    [SerializeField] private LayerMask _groundLayerMask;

    [SerializeField] private float distanceAvecLeSol;

    private Rigidbody _rb;
    private PlayerInputs _frameInput;
    private InteractionModule _interactionModule;

    private bool _hasControl;
    private bool _grounded;
    private bool _facingRight;
    private float _gravityScale;

    #region Public

    public bool DisabledControls;

    public Vector2 Speed => new (_rb.velocity.x, _rb.velocity.z);
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
        bool groundedLive = Physics.Raycast(transform.position, Vector3.down, distanceAvecLeSol, _groundLayerMask);
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
        float targetSpeed = (_frameInput.Movement2d.Live.x == 0 ? 0 : MathF.Sign(_frameInput.Movement2d.Live.x)) * _stats.MoveSpeed;
        // calculate difference between current volocity and target velocity
        float vel = 0;
        if (GameManager.Inst.TowerSide == TowerContext.South) vel = _rb.velocity.x;
        if (GameManager.Inst.TowerSide == TowerContext.East) vel = _rb.velocity.z;
        if (GameManager.Inst.TowerSide == TowerContext.North) vel = -_rb.velocity.x;
        if (GameManager.Inst.TowerSide == TowerContext.West) vel = -_rb.velocity.z;
        float speedDif = targetSpeed - vel;
        // change acceleration rate depending on situations;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Decceleration;
        // applies acceleration to speed difference, raise to a set power so acceleration increase with higher speed
        // multiply by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);

        // apply the movement force
        _rb.AddForce(movement * GameManager.Inst.ConvertVector(Vector3.right));
    }
    #endregion

    // private void HorizontalTransform

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
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
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

    public void FreezePosition(FreezePositionAxis axis)
    {
        _rb.constraints = axis switch
        {
            FreezePositionAxis.X => RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX,
            FreezePositionAxis.Z => RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ,
            _ => _rb.constraints
        };
    }

    public void ResetVelocity()
    {
        _rb.velocity = Vector3.zero;
    }
}

public enum FreezePositionAxis { X, Z }