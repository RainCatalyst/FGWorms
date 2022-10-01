using UnityEngine;

namespace FGWorms.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
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
        private float _groundDistance = 0.1f;

        // State
        private CharacterController _controller;
        private bool _isGrounded;
        private float _verticalVelocity;
        private Vector3 _currentDirection;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void Move()
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
            if (Physics.Raycast(_groundCheck.position, Vector3.down,
                    out RaycastHit hitInfo, _groundDistance, _groundLayers))
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
            
            if (_isGrounded)
            {
                _verticalVelocity = 0;
            }
        }
    }
}