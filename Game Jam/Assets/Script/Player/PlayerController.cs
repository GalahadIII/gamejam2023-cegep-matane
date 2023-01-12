// using System;
// using UnityEngine;
//
// namespace Player
// {
//     [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
//     public class PlayerController : MonoBehaviour, IPlayerController
//     {
//         [SerializeField] private PlayerMoveStats _stats;
//
//         #region Private
//
//         [SerializeField] private LayerMask _groundLayerMask; 
//         
//         private Rigidbody2D _rb;
//         private InputManager _input;
//         private FrameInput _frameInput;
//         
//         private int _fixedUpdateCounter;
//         private bool _hasControl;
//         private bool _grounded;
//         private bool _facingRight;
//         private float _gravityScale;
//         private BoxCollider2D _feetCollider;
//         private CapsuleCollider2D _headCollider;
//
//         #endregion
//
//         #region Public
//
//         public Vector2 Input => _frameInput.Move;
//         public Vector2 Speed => _rb.velocity;
//         public bool Falling => _rb.velocity.y < 0;
//         public bool Jumping => _rb.velocity.y > 0;
//         public bool FacingRight => _facingRight;
//         public bool Grounded => _grounded;
//         public bool Rolling => _rolling;
//         public bool Dashing => _dashing;
//
//         #endregion
//
//         private void Awake()
//         {
//             _rb = GetComponent<Rigidbody2D>();
//             _input = GetComponent<InputManager>();
//             _feetCollider = GetComponent<BoxCollider2D>();
//         }
//
//         private void Start()
//         {
//             _gravityScale = _rb.gravityScale;
//         }
//
//         private void Update()
//         {
//             IsGrounded();
//                         
//             _frameInput = _input.FrameInput;
//             if (_frameInput.JumpDown)
//             {
//                 _jumpToConsume = true;
//                 _frameJumpWasPressed = _fixedUpdateCounter;
//             }
//             if (_frameInput.JumpUp) JumpCut();
//
//             if (_frameInput.DashDown && (_stats.AllowDash || _stats.AllowRoll)) _dodgeToConsume = true;
//         }
//         
//         private void FixedUpdate()
//         {
//             _fixedUpdateCounter++;
//
//             // input dependant
//             Flip();
//             Horizontal();
//             Jump();
//             Dodge();
//             ArtificialFriction();
//             
//             
//             // !input dependant
//             GravityModifier();
//         }
//
//         #region Checks
//
//         private void IsGrounded()
//         {
//             float extraHeightText = 0.030f;
//             Bounds bounds = _feetCollider.bounds;
//             
//             RaycastHit2D raycastHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, extraHeightText,
//                 _groundLayerMask);
//
//             if (!raycastHit && _grounded)
//             {
//                 _frameLeftGround = _fixedUpdateCounter;
//             }
//             else if (raycastHit && !_grounded)
//             {
//                 ResetJump();
//             }
//             
//             _grounded = raycastHit;
//         }
//
//         #endregion
//
//         #region Horizontal
//
//         private void Horizontal()
//         {
//             if (_dashing || _rolling) return;
//
//             // calculate wanted direction and desired velocity
//             float targetSpeed = (_frameInput.Move.x == 0 ? 0 : MathF.Sign(_frameInput.Move.x))  * _stats.MoveSpeed;
//             // calculate difference between current volocity and target velocity
//             float speedDif = targetSpeed - _rb.velocity.x;
//             // change acceleration rate depending on situations;
//             float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Decceleration;
//             // applies acceleration to speed difference, raise to a set power so acceleration increase with higher speed
//             // multiply by sign to reapply direction
//             float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);
//
//             // apply the movement forcea 
//             _rb.AddForce(movement * Vector2.right);
//         }
//
//         private void ArtificialFriction()
//         {
//             if (!_grounded) return;
//             if (!(Mathf.Abs(_frameInput.Move.x) < 0.01f)) return;
//             
//             // use either friction amount or velocity
//             float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_stats.Friction));
//             // sets to movement direction
//             amount *= Mathf.Sign(_rb.velocity.x);
//             // applies force against movement direction
//             _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
//
//         }
//
//         #endregion
//
//         #region Jump
//
//         private bool _jumpToConsume;
//         private int _frameJumpWasPressed;
//         private int _frameLeftGround;
//
//         private bool _bufferedJumpUsable;
//         private bool _coyoteUsable;
//         
//         
//         private bool HasBufferedJump =>
//             _bufferedJumpUsable && _fixedUpdateCounter < _frameJumpWasPressed + _stats.JumpBufferFrame;
//         private bool CanUseCoyote => 
//             _coyoteUsable && !_grounded && _fixedUpdateCounter < _frameLeftGround + _stats.JumpCoyoteFrame;
//
//         private void Jump()
//         {
//             if (_jumpToConsume || HasBufferedJump)
//             {
//                 if (_grounded || CanUseCoyote) NormalJump();
//             }
//             _jumpToConsume = false;
//         }
//
//         private void NormalJump()
//         {
//             _coyoteUsable = false;
//             _bufferedJumpUsable = false;
//             _rb.velocity = new Vector2(_rb.velocity.x, 0);
//             _rb.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);
//         }
//
//         private void ResetJump()
//         {
//             _coyoteUsable = true;
//             _bufferedJumpUsable = true;
//         }
//
//         private void JumpCut()
//         {
//             if (!Falling)
//             {
//                 _rb.AddForce(Vector2.down * (_rb.velocity.y * (1 - _stats.JumpCutMultiplier)), ForceMode2D.Impulse);
//             }
//         }
//
//         private void GravityModifier()
//         {
//             _rb.gravityScale = Falling ? _stats.FallGravityMultiplier : _gravityScale;
//         }
//
//         #endregion
//
//         #region Dodge
//         
//         private bool _dodgeToConsume;
//         private int _frameDodgeWasPressed;
//         private float dodgeDir;
//         
//         private bool _dashing;
//         private bool _rolling;
//
//         private void Dodge()
//         {
//             if (_dodgeToConsume)
//             {
//                 _frameDodgeWasPressed = _fixedUpdateCounter;
//                 _dodgeToConsume = false;
//                 float moveX = _frameInput.Move.x;
//                 if (moveX.Equals(0)) return;
//                 dodgeDir = MathF.Sign(moveX);
//
//                 if (_grounded && _stats.AllowRoll) _rolling = true;
//                 else if (!_grounded && _stats.AllowDash) _dashing = true;
//             }
//             
//             if (_dashing) Dash();
//             if (_rolling) Roll();
//         }
//         
//         private void Dash()
//         {
//             if (_fixedUpdateCounter > _frameDodgeWasPressed + _stats.DashDurationFrame)
//             {
//                 StopDodge();
//                 return;
//             }
//
//             _dashing = true;
//             _rb.velocity = new Vector2(_rb.velocity.x, 0);
//             float dashVel = dodgeDir * _stats.DashVelocity;
//             _rb.AddForce(dashVel * Vector2.right);
//             
//         }
//
//         private void Roll()
//         {
//             if (_fixedUpdateCounter > _frameDodgeWasPressed + _stats.RollDurationFrame)
//             {
//                 StopDodge();
//                 return;
//             }
//
//             _rolling = true;
//             float rollVel = dodgeDir * _stats.RollVelocity;
//             _rb.AddForce(rollVel * Vector2.right);
//         }
//         
//         private void StopDodge()
//         {
//             _rolling = false;
//             _dashing = false;
//         }
//
//         #endregion
//
//         private void Flip()
//         {
//             if (Mathf.Abs(_frameInput.Move.x) < 0.1f) return;
//             if (_facingRight && Mathf.Sign(_frameInput.Move.x) < 0) return;
//             if (!_facingRight && Mathf.Sign(_frameInput.Move.x) > 0) return;
//             Utilities.FlipTransform(transform);
//             _facingRight = !_facingRight;
//         }
//     }
//
//     public interface IPlayerController
//     {
//         public Vector2 Input { get; }
//         public Vector2 Speed { get; }
//         public bool Falling { get; }
//         public bool Jumping { get; }
//         public bool Rolling { get; }
//         public bool Dashing { get; }
//         public bool Grounded { get; }
//     }
// }
