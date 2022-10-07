using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        public UnityAction Jumped;
        public UnityAction Landed;
        
        public void SetMoveAxis(Vector2 axis) => _inputAxis = axis;

        public void SetJump(float strength)
        {
            _inputJump = true;
            _inputJumpStrength = strength;
        }

        public void FaceForward(bool instant)
        {
            _targetRotation = Quaternion.LookRotation(GetForward());
            if (instant)
            {
                transform.rotation = _targetRotation;
            }
        }

        public void SetVelocity(Vector3 velocity)
        {
            _verticalVelocity = velocity;
            _isGrounded = false;
            _isSloped = false;
            _controller.Move(Vector3.up * _groundCheckDistance);
        }

        private void Awake()
        {
            // Fetch references
            _controller = GetComponent<CharacterController>();
            _targetRotation = transform.rotation;
        }

        private void Start()
        {
            _forwardSource = LevelManager.Instance.Camera.transform;
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
            Move();
            Rotate();
        }
        
        private void CheckGround()
        {
            float checkRadius = _controller.radius * 0.75f;
            float checkHeight = _controller.height * 0.5f;
            bool isNowGrounded = Physics.SphereCast(
                transform.position + checkHeight * Vector3.up, checkRadius,
                Vector3.down, out RaycastHit hitInfo, checkHeight - checkRadius + _groundCheckDistance, _groundLayers);

            _groundNormal = isNowGrounded ? hitInfo.normal : Vector3.up;

            if (isNowGrounded && Vector3.Dot(_groundNormal, -GravityDirection) < _slopeLimit)
            {
                isNowGrounded = false;
                _isSloped = true;
            }
            else
            {
                _isSloped = false;
            }
            
            if ((isNowGrounded || _isSloped) && !_isGrounded)
            {
                // Snap to ground
                _controller.Move(Vector3.down * (transform.position.y - hitInfo.point.y));
                Landed?.Invoke();
            }

            _isGrounded = isNowGrounded;
        }
        
        private void Move()
        {
            if (_isGrounded)
                _verticalVelocity = _gravity * GravityDirection;

            float verticalSpeed = Vector3.Dot(GravityDirection, _controller.velocity);
            float gravityMultiplier = 1.0f;
            if (verticalSpeed < 0f)
                gravityMultiplier += _fallMultiplier;
            _verticalVelocity += Time.deltaTime * _gravity * gravityMultiplier * GravityDirection;

            if (_inputJump)
            {
                if (_isGrounded)
                    Jump();
                _inputJump = false;
            }
            
            float currentAcceleration = _isGrounded ? _groundAcceleration : _airAcceleration;
            Vector3 targetHorizontalVelocity = (GetRight() * _inputAxis.x + GetForward() * _inputAxis.y) * _walkSpeed;
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, targetHorizontalVelocity,
                Time.deltaTime * currentAcceleration);

            if (_isSloped)
            {
                var projectedGravity = Vector3.ProjectOnPlane(GravityDirection, _groundNormal);
                _verticalVelocity = _slideGravityMultiplier * _gravity * projectedGravity.normalized;
                _horizontalVelocity *= _slideSpeedMultiplier;
            }

            // Move using computed velocities
            _controller.Move((_horizontalVelocity + _verticalVelocity) * Time.deltaTime);
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
            // Callback
            Jumped?.Invoke();
        }

        // Callbacks
        private void OnJumped()
        {
            _isGrounded = false;
            _isSloped = false;
            _groundNormal = Vector3.up;
        }

        private void OnLanded()
        {
            if (_isSloped)
            {
                // Project current velocity onto the ground plane when landing on slope
                var projectedVelocity = Vector3.ProjectOnPlane(_verticalVelocity, _groundNormal);
                _verticalVelocity = projectedVelocity.normalized * _verticalVelocity.magnitude;
            }
        }
        
        private Vector3 GetForward() => new Vector3(_forwardSource.forward.x, 0, _forwardSource.forward.z).normalized;
        private Vector3 GetRight() => new Vector3(_forwardSource.right.x, 0, _forwardSource.right.z).normalized;
        
        [Header("Walk Settings")]
        [SerializeField]
        private float _walkSpeed = 4.0f;
        [SerializeField]
        private float _slideSpeedMultiplier = 0.5f;
        [SerializeField]
        private float _groundAcceleration = 50.0f;

        [Header("Jump Settings")]
        [SerializeField]
        private float _jumpHeight = 1.5f;
        [SerializeField]
        private float _jumpDistance = 3f;
        [SerializeField]
        private float _airAcceleration = 0.5f;
        [SerializeField]
        private float _fallMultiplier = 0.5f;
        [SerializeField]
        private float _slopeLimit = 0.95f;

        [Header("Rotation Settings")]
        [SerializeField]
        private float _rotationSmooth = 18f;

        [Header("Gravity Settings")]
        [SerializeField]
        private float _gravity = 30.0f;
        [SerializeField]
        private float _slideGravityMultiplier = 0.2f;

        [Header("Ground Settings")]
        [SerializeField]
        private LayerMask _groundLayers;
        [SerializeField]
        private float _groundCheckDistance = 0.1f;
        
        // Components
        private Transform _forwardSource;
        private CharacterController _controller;
        
        // Input
        private Vector2 _inputAxis;
        private bool _inputJump;
        private float _inputJumpStrength;

        // Internal
        private Vector3 _velocity;
        private Vector3 _horizontalVelocity;
        private Vector3 _verticalVelocity;
        private Quaternion _targetRotation;
        private Vector3 _groundNormal = Vector3.up;
        private bool _isGrounded;
        private bool _isSloped;

        readonly Vector3 GravityDirection = Vector3.down;
    }
}