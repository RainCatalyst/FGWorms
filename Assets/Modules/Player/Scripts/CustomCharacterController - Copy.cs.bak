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
    [SerializeField] private float _airAcceleration = 8.0f;
    [SerializeField] private float _fallMultiplier = 0.5f;
    [SerializeField] private float _jumpMultiplier = 0.5f;
    [SerializeField] private float _jumpQueueTime = 0.1f;
    [SerializeField] private float _coyoteJumpTime = 0.1f;

    [Header("Gravity Settings")]
    [SerializeField] private float _gravity = 30.0f;
    [SerializeField] private float _maxVelocity = 30f;

    [Header("Ground Settings")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private float _slipCheckDistance = 0.2f;
    [SerializeField] private float _slipSpeed = 1f;

    public UnityAction Jumped;
    public UnityAction Landed;
    
    public bool IsGrounded => _isGrounded;

    // Components
    private CharacterController _controller;

    // Input
    private Vector2 _inputAxis;
    private Vector2 _mouseAxis;
    private Vector2 _turnAxis;
    private bool _inputHoldJump;
    private bool _inputJump;

    // Variables
    private Vector3 _velocity;
    private Vector3 _smoothHorizontalVelocity;
    private Vector3 _horizontalVelocity;
    private Vector3 _verticalVelocity;
    private Vector3 _groundNormal = Vector3.up;
    private Vector3 _slopeNormal = Vector3.up;
    private bool _isGrounded;
    private bool _isSloped;
    private bool _isCeiled;
    
    private Vector3 GetForward() => new Vector3(forwardSource.forward.x, 0, forwardSource.forward.z).normalized;
    private Vector3 GetRight() => new Vector3(forwardSource.right.x, 0, forwardSource.right.z).normalized;
    
    public void SetMoveAxis(Vector2 axis) => _inputAxis = axis;
    public void SetJump() => _inputJump = true;

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
        // CheckSlopes();
        // CheckCeiling();
        HorizontalMove();
        VerticalMove();
        Move();
    }

    private void CheckGround()
    {
        RaycastHit hitInfo;
        float checkDistance = (_controller.height * 0.5f + _controller.skinWidth + _groundCheckDistance);
        bool isNowGrounded = Physics.Raycast(
            transform.position + _controller.center,
            Vector3.down, out hitInfo, checkDistance, _groundLayers);

        _groundNormal = isNowGrounded ? hitInfo.normal : Vector3.up;

        if (isNowGrounded && !_isGrounded) {
            // Snap to ground
            _controller.Move(Vector3.down * (transform.position.y - hitInfo.point.y));
            Landed?.Invoke();
        }
            
        _isGrounded = isNowGrounded;

        #if UNITY_EDITOR
            Debug.DrawLine(transform.position + _controller.center, transform.position + _controller.center + Vector3.down * checkDistance, Color.blue);
        #endif
    }

    private void CheckSlopes()
    {
        RaycastHit hitInfo;
        float checkRadius = _controller.radius;
        float checkDistance = (_controller.skinWidth + _groundCheckDistance);
        bool isNowSloped = Physics.SphereCast(
            transform.position + Vector3.up * (checkRadius - _controller.height * 0.5f), checkRadius,
            Vector3.down, out hitInfo, checkDistance, _groundLayers);
        if (isNowSloped)
            _slopeNormal = hitInfo.normal;
        else
            _slopeNormal = Vector3.up;

        _isSloped = isNowSloped;
    }

    private void CheckCeiling()
    {
        RaycastHit hitInfo;
        float checkDistance = (_controller.height * 0.5f + _controller.skinWidth + _groundCheckDistance);
        bool isNowCeiled = Physics.Raycast(
            transform.position + _controller.center,
            Vector3.up, out hitInfo, checkDistance, _groundLayers);
        if (isNowCeiled && !_isCeiled)
            OnCeiled();
        _isCeiled = isNowCeiled;

        #if UNITY_EDITOR
            Debug.DrawLine(transform.position + _controller.center, transform.position + _controller.center + Vector3.up * checkDistance, Color.blue);
        #endif
    }

    private void CheckEdge() {
        RaycastHit hit;
        Vector3 castSource = transform.position - Vector3.up * (_controller.skinWidth + _controller.height * 0.5f);
        float castDistance = _controller.radius + _slipCheckDistance;

        Vector3 Forward = GetForward();
        Vector3 Right = GetRight();
        if (Physics.Raycast(castSource, Forward, out hit, castDistance, _groundLayers)){
            FixSlip(hit.normal);
        }
        if (Physics.Raycast(castSource, -Forward, out hit, castDistance, _groundLayers)
            || Physics.Raycast(castSource, Right, out hit, castDistance, _groundLayers)
            || Physics.Raycast(castSource, -Right, out hit, castDistance, _groundLayers))
        {
            FixSlip(hit.normal);
        }
    }

    private void FixSlip(Vector3 direction) {
        _controller.Move((direction * _slipSpeed + Vector3.down * 0.5f) * Time.deltaTime);
    }

    private void HorizontalMove()
    {
        float moveSmooth = _isGrounded ? _groundAcceleration : _airAcceleration;

        Vector3 targetHorizontalVelocity = (GetRight() * _inputAxis.x + GetForward() * _inputAxis.y) * _walkSpeed;
        _smoothHorizontalVelocity = Vector3.Lerp(_smoothHorizontalVelocity, targetHorizontalVelocity, Time.deltaTime * moveSmooth);
        _horizontalVelocity = _smoothHorizontalVelocity;
    }

    private void VerticalMove()
    {
        if (_inputJump)
        {
            if (IsGrounded)
                Jump();
            _inputJump = false;
        }

        if (_isGrounded)
            _verticalVelocity = Vector3.zero;

        float verticalSpeed = Vector3.Dot(gravityDirection, _controller.velocity);
        float gravityMultiplier = 1.0f;
        if (verticalSpeed > 0f)
            gravityMultiplier += _fallMultiplier;
        else if (verticalSpeed < 0f && !_inputHoldJump)
            gravityMultiplier += _jumpMultiplier;
        _verticalVelocity += Time.deltaTime * _gravity * gravityMultiplier * gravityDirection;
    }

    private void Move() 
    {
        if (_isGrounded) {
            // Project horizontal velocity onto ground plane
            var projectedVelocity = Vector3.ProjectOnPlane(_horizontalVelocity, _groundNormal);
            _horizontalVelocity = projectedVelocity.normalized * _horizontalVelocity.magnitude;
            // Fix vertical slope movement
            if (_horizontalVelocity.y > 0f)
                _horizontalVelocity -= _verticalVelocity;
        }else if (_isSloped && Vector3.Angle(Vector3.up, _slopeNormal) > _controller.slopeLimit) {
            // Sliding movement
            _horizontalVelocity *= _slideVelocityMultiplier;
            var projectedVerticalVelocity = Vector3.ProjectOnPlane(_verticalVelocity, _slopeNormal);
            _verticalVelocity = projectedVerticalVelocity;
        }

        if (!_isGrounded && _controller.velocity.y < 0) {
            CheckEdge();
        }

        // Move using computed velocities
        _controller.Move((_horizontalVelocity + _verticalVelocity) * Time.deltaTime);
    }

    public void Jump()
    {
        // Move up by ground check distance to avoid hitting ground next frame (if jump velocity is too low)
        _controller.Move(Vector3.up * _groundCheckDistance);
        // Compute the vertical velocity needed to reach jump height
        _verticalVelocity = Vector3.up * Mathf.Sqrt(2f * _jumpHeight * _gravity);
        // Callback
        Jumped?.Invoke();
    }

    // Callbacks
    private void OnJumped()
    {
        // Reset isGrounded flag
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

    private void OnCeiled() {
        // Reset velocity when touching ceiling
        _verticalVelocity = Vector3.zero;
    }
}