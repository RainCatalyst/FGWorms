using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float _speedSmooth = 16f;
    [SerializeField]
    private float _jumpSpeed = 10f;
    
    // [Header("Rotation")]
    // private float _rotationSmooth = 12f;
    
    [Header("Ground")]
    [SerializeField]
    private LayerMask _groundLayers;
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _groundDistance = 0.1f;
    
    // State
    private Vector3 _currentDirection;
    private bool _isGrounded;
    private RaycastHit _groundInfo = new() { normal = Vector3.up };
    private int _stepsSinceGrounded;
    private Rigidbody _rb;
    
    // Input
    private bool _inputJump;
    private Vector2 _inputMove;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        UpdateGround();
        _stepsSinceGrounded += 1;
        if (_isGrounded)
        {
            _stepsSinceGrounded = 0;
        }
        // Movement
        Vector3 direction = GetForward() * _inputMove.y + GetRight() * _inputMove.x;
        _currentDirection = Vector3.Lerp(_currentDirection, direction, _speedSmooth * Time.fixedDeltaTime);

        float verticalVelocity = Vector3.Dot(_rb.velocity, _groundInfo.normal);
        var oldVelocity = _rb.velocity;
        Vector3 velocity = direction * _speed + _groundInfo.normal * verticalVelocity;
        _rb.velocity = velocity;
        
        // Realign with ground
        if (_stepsSinceGrounded == 1)
        {
            Vector3 groundNormal = GetClosestGroundNormal();
            float speed = oldVelocity.magnitude;
            float dot = Vector3.Dot(oldVelocity, groundNormal);
            if (dot > 0f)
                _rb.velocity = (oldVelocity - groundNormal * dot).normalized * speed;
        }

        // Jumping
        if (_inputJump)
        {
            _inputJump = false;
            // Jump
            Jump();
        }
    }

    private void Update()
    {
        // Process input
        _inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _inputJump |= Input.GetButtonDown("Jump");
    }
    
    private void Jump()
    {
        _rb.velocity += _groundInfo.normal * _jumpSpeed;
    }
    
    // private void Rotate()
    // {
    //     var forward = GetForward();
    //     float angle = Mathf.Acos(forward.z) * Mathf.Rad2Deg;
    //     angle = forward.x < 0f ? 360f - angle : angle;
    //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), _rotationSmooth * Time.deltaTime);
    // }

    private void UpdateGround()
    {
        if (Physics.Raycast(_groundCheck.position, Vector3.down, 
                out RaycastHit hitInfo, _groundDistance, _groundLayers))
        {
            _isGrounded = true;
            _groundInfo = hitInfo;
        }
        else
        {
            _isGrounded = false;
            _groundInfo = new RaycastHit() { normal = Vector3.up };
        }
    }

    private Vector3 GetClosestGroundNormal()
    {
        if (Physics.Raycast(_groundCheck.position, Vector3.down, 
                out RaycastHit hitInfo, _groundDistance * 100f, _groundLayers))
        {
            return hitInfo.normal;
        }
        return Vector3.up;
    }
}
