using System;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 649, 414
[RequireComponent(typeof(CharacterController))]
public class CustomCharacterController : MonoBehaviour
{
    readonly Vector3 gravityDirection = Vector3.down;
    
    [SerializeField] Transform forwardSource;

    [Header("Walk Settings")]
    [SerializeField] private float _walkSpeed = 4.0f;
    [SerializeField] private float _groundAcceleration = 50.0f;
    [SerializeField] private float _slideVelocityMultiplier = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _jumpDistance = 3f;
    [SerializeField] private float _airAcceleration = 0.5f;
    [SerializeField] private float _fallMultiplier = 0.5f;
    [SerializeField] private float _jumpMultiplier = 0.5f;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSmooth = 18f;

    [Header("Gravity Settings")]
    [SerializeField] private float _gravity = 30.0f;

    [Header("Ground Settings")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] private float _groundCheckDistance = 0.1f;

    public UnityAction Jumped;
    public UnityAction Landed;
    
    public bool IsGrounded => _isGrounded;

    // Components
    private CharacterController _controller;

    // Input
    private Vector2 _inputAxis;
    private bool _inputHoldJump;
    private bool _inputJump;
    private float _inputJumpStrength;
    private bool JustJumped;

    // Variables
    private Vector3 _velocity;
    private Vector3 _smoothHorizontalVelocity;
    private Vector3 _horizontalVelocity;
    private Vector3 _verticalVelocity;
    private Quaternion _targetRotation;
    private Vector3 _groundNormal = Vector3.up;
    private Vector3 _slopeNormal = Vector3.up;
    private bool _isGrounded;
    private bool _isSloped;
    private bool _isCeiled;
    
    private Vector3 GetForward() => new Vector3(forwardSource.forward.x, 0, forwardSource.forward.z).normalized;
    private Vector3 GetRight() => new Vector3(forwardSource.right.x, 0, forwardSource.right.z).normalized;
    
    public void SetMoveAxis(Vector2 axis) => _inputAxis = axis;

    public void SetJump(float strength)
    { 
        _inputJump = true;
        _inputJumpStrength = strength;
    }

    public void FaceForward()
    {
        _targetRotation = Quaternion.LookRotation(GetForward());
    }


    private void Awake()
    {
        // Fetch references
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        Jumped += OnJumped;
        Landed += OnLanded;
    }

    private void OnDisable()
    {
        Jumped -= OnJumped;
        Landed -= OnLanded;
    }

    private void Update()
    {
        CheckGround();
        ComputeHorizontalVelocity();
        ComputeVerticalVelocity();
        Move();
        Rotate();
    }

    private void CheckGround()
    {
        RaycastHit hitInfo;
        float checkDistance = (_controller.height * 0.5f + _controller.skinWidth + _groundCheckDistance);
        bool isNowGrounded = Physics.Raycast(
            transform.position + _controller.center,
            Vector3.down, out hitInfo, checkDistance, _groundLayers);

        _groundNormal = isNowGrounded ? hitInfo.normal : Vector3.up;

        if (isNowGrounded && !_isGrounded)
        {
            // Snap to ground
            _controller.Move(Vector3.down * (transform.position.y - hitInfo.point.y));
            Landed?.Invoke();
        }
            
        _isGrounded = isNowGrounded;

        #if UNITY_EDITOR
            Debug.DrawLine(transform.position + _controller.center, transform.position + _controller.center + Vector3.down * checkDistance, Color.blue);
        #endif
    }

    private void ComputeHorizontalVelocity()
    {
        float moveSmooth = _isGrounded ? _groundAcceleration : _airAcceleration;

        Vector3 targetHorizontalVelocity = (GetRight() * _inputAxis.x + GetForward() * _inputAxis.y) * _walkSpeed;
        _smoothHorizontalVelocity = Vector3.Lerp(_smoothHorizontalVelocity, targetHorizontalVelocity, Time.deltaTime * moveSmooth);
        _horizontalVelocity = _smoothHorizontalVelocity;
    }

    private void ComputeVerticalVelocity()
    {
        if (_isGrounded)
            _verticalVelocity = Vector3.zero;

        float verticalSpeed = Vector3.Dot(gravityDirection, _controller.velocity);
        float gravityMultiplier = 1.0f;
        if (verticalSpeed > 0f)
            gravityMultiplier += _fallMultiplier;
        else if (verticalSpeed < 0f && !_inputHoldJump)
            gravityMultiplier += _jumpMultiplier;
        _verticalVelocity += Time.deltaTime * _gravity * gravityMultiplier * gravityDirection;
        
        if (_inputJump)
        {
            if (IsGrounded)
                Jump();
            _inputJump = false;
        }
    }

    private void Move() 
    {
        if (_isGrounded)
        {
            // Project horizontal velocity onto ground plane
            var projectedVelocity = Vector3.ProjectOnPlane(_horizontalVelocity, _groundNormal);
            _horizontalVelocity = projectedVelocity.normalized * _horizontalVelocity.magnitude;
            // Fix vertical slope movement
            if (_horizontalVelocity.y > 0f)
                _horizontalVelocity -= _verticalVelocity;
        }
        else if (_isSloped && Vector3.Angle(Vector3.up, _slopeNormal) > _controller.slopeLimit)
        {
            // Slide
            _horizontalVelocity *= _slideVelocityMultiplier;
            var projectedVerticalVelocity = Vector3.ProjectOnPlane(_verticalVelocity, _slopeNormal);
            _verticalVelocity = projectedVerticalVelocity;
        }
        
        // Move using computed velocities
        _velocity = (_horizontalVelocity + _verticalVelocity) * Time.deltaTime;
        _controller.Move(_velocity);
    }

    private void Rotate()
    {
        if (_horizontalVelocity.magnitude > 1e-2f)
        {
            Vector3 flatVelocity = new Vector3(_horizontalVelocity.x, 0, _horizontalVelocity.z);
            _targetRotation = Quaternion.LookRotation(flatVelocity);
        }
        transform.rotation = Quaternion.Lerp(
            transform.rotation, _targetRotation, Time.deltaTime * _rotationSmooth);
    }

    private void Jump()
    {
        // Move up by ground check distance to avoid hitting ground next frame (if jump velocity is too low)
        _controller.Move(Vector3.up * _groundCheckDistance);
        // Compute the vertical velocity needed to reach jump height
        _verticalVelocity = Mathf.Sqrt(2f * _jumpHeight * _gravity) * _inputJumpStrength * Vector3.up;
        _horizontalVelocity = _jumpDistance * _inputJumpStrength * GetForward();
        _smoothHorizontalVelocity = _horizontalVelocity;
        // Callback
        Jumped?.Invoke();
    }

    // Callbacks
    private void OnJumped()
    {
        // Reset isGrounded flag
        JustJumped = true;
        _isGrounded = false;
        _isSloped = false;
        _groundNormal = Vector3.up;
    }

    private void OnLanded()
    {
        // Project current velocity onto the ground plane
        var projectedVelocity = Vector3.ProjectOnPlane(_verticalVelocity, _groundNormal);
        _verticalVelocity = projectedVelocity.normalized * _verticalVelocity.magnitude;
    }
}