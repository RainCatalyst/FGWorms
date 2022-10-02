using System;
using UnityEngine;

namespace FGWorms.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementSimple : MonoBehaviour
    {
        private Vector3 GetForward() => new Vector3(_inputSpace.forward.x, 0, _inputSpace.forward.z).normalized;
        private Vector3 GetRight() => new Vector3(_inputSpace.right.x, 0, _inputSpace.right.z).normalized;

        [SerializeField]
        private Transform _inputSpace;

        [Header("Movement")]
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _gravity = 10f;
        [SerializeField]
        private float _speedSmooth = 16f;
        [SerializeField]
        private float _rotationSmooth = 12f;
        [SerializeField]
        private float _jumpSpeed = 1f;

        [Header("Ground")]
        [SerializeField]
        private LayerMask _groundLayers;
        [SerializeField]
        private Transform _groundCheck;
        [SerializeField]
        private float _groundCheckDistance = 0.1f;
        [SerializeField]
        private float _groundCheckRadius = 0.05f;

        // State
        private CharacterController _controller;
        private bool _isGrounded;
        private float _verticalVelocity;
        private Vector3 _currentDirection;
        private bool _wasGrounded;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void Move(Vector2 moveInput)
        {
            Vector3 direction = GetForward() * moveInput.y + GetRight() * moveInput.x;
            _currentDirection = Vector3.Lerp(_currentDirection, direction, _speedSmooth * Time.fixedDeltaTime);

            _controller.Move(_speed * Time.deltaTime * _currentDirection);
            _verticalVelocity += _gravity * Time.deltaTime;
            _controller.Move(_verticalVelocity * Time.deltaTime * Vector3.down);
        }

        public void Rotate()
        {
            var forward = GetForward();
            float angle = Mathf.Acos(forward.z) * Mathf.Rad2Deg;
            angle = forward.x < 0f ? 360f - angle : angle;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), _rotationSmooth * Time.deltaTime);
        }

        public void UpdateGround()
        {
            // _isGrounded = Physics.SphereCast(_groundCheck.position, _groundCheckRadius,
            //     Vector3.down, out RaycastHit hitInfo, _groundCheckDistance, _groundLayers);
            _isGrounded = IsGrounded();

            if (!_isGrounded && _wasGrounded)
            {
                // Move down
                _controller.Move( _gravity * Time.deltaTime * Vector3.down);
                // Recheck grounded
                _isGrounded = IsGrounded();
            }
            
            print($"Raycast: {_isGrounded}, Body: {_controller.isGrounded}");
            
            _wasGrounded = _isGrounded;
            // Reset velocity when grounded
            if (_isGrounded)
                _verticalVelocity = 0;
        }

        private bool IsGrounded()
        {
            return Physics.SphereCast(_groundCheck.position, _groundCheckRadius,
            Vector3.down, out RaycastHit hitInfo, _groundCheckDistance, _groundLayers);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + Vector3.down * _groundCheckDistance);
            Gizmos.DrawWireSphere(_groundCheck.position + Vector3.down * _groundCheckDistance, _groundCheckRadius);
        }
    }
}