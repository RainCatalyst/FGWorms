using UnityEngine;

#pragma warning disable 649, 414
[RequireComponent(typeof(CharacterController))]
public class CustomCharacterController : MonoBehaviour
{
    readonly Vector3 gravityDirection = Vector3.down;
    
    [SerializeField] Transform forwardSource;

    // Parameters
    [Header("Walk Settings")]
    [SerializeField] float walkSpeed = 4.0f;
    [SerializeField] float groundAcceleration = 50.0f;
    [SerializeField] float slideVelocityMultiplier = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float airAcceleration = 8.0f;
    [SerializeField] float fallMultiplier = 0.5f;
    [SerializeField] float jumpMultiplier = 0.5f;
    [SerializeField] float jumpQueueTime = 0.1f;
    [SerializeField] float coyoteJumpTime = 0.1f;

    [Header("Gravity Settings")]
    [SerializeField] float gravity = 30.0f;
    [SerializeField] float maxVelocity = 30f;

    [Header("Ground Settings")]
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] float slipCheckDistance = 0.2f;
    [SerializeField] float slipSpeed = 1f;

    // Public fields
    public bool IsGrounded => coyoteJumpTimer < coyoteJumpTime;
    public bool IsCeiled => isCeiled;
    public bool IsSloped => isSloped;

    // Components
    CharacterController controller;

    // Input
    Vector2 inputAxis;
    Vector2 mouseAxis;
    Vector2 turnAxis;
    bool inputHoldJump;
    bool inputJump;

    // Variables
    Quaternion headRotation = Quaternion.identity;
    Vector3 velocity;
    Vector3 smoothHorizontalVelocity;
    Vector3 horizontalVelocity;
    Vector3 verticalVelocity;
    Vector3 groundNormal = Vector3.up;
    Vector3 slopeNormal = Vector3.up;
    bool isGrounded;
    bool isSloped;
    bool isCeiled;
    float jumpQueueTimer;
    float coyoteJumpTimer;
    bool jumpQueued;

    public void SetMoveAxis(Vector2 axis) => inputAxis = axis;

    void Awake()
    {
        // Fetch references
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckGround();
        CheckSlopes();
        CheckCeiling();
        HorizontalMove();
        VerticalMove();
        Move();
    }

    void CheckGround()
    {
        RaycastHit hitInfo;
        float checkDistance = (controller.height * 0.5f + controller.skinWidth + groundCheckDistance);
        bool isNowGrounded = Physics.Raycast(
            transform.position + controller.center,
            Vector3.down, out hitInfo, checkDistance, groundLayers);

        groundNormal = isNowGrounded ? hitInfo.normal : Vector3.up;

        if (isNowGrounded && !isGrounded) {
            // Snap to ground
            controller.Move(Vector3.down * (transform.position.y - hitInfo.point.y));
            OnLanded();
        }
            
        isGrounded = isNowGrounded;

        #if UNITY_EDITOR
            Debug.DrawLine(transform.position + controller.center, transform.position + controller.center + Vector3.down * checkDistance, Color.blue);
        #endif
    }

    void CheckSlopes()
    {
        RaycastHit hitInfo;
        float checkRadius = controller.radius;
        float checkDistance = (controller.skinWidth + groundCheckDistance);
        bool isNowSloped = Physics.SphereCast(
            transform.position + Vector3.up * (checkRadius - controller.height * 0.5f), checkRadius,
            Vector3.down, out hitInfo, checkDistance, groundLayers);
        if (isNowSloped)
            slopeNormal = hitInfo.normal;
        else
            slopeNormal = Vector3.up;

        isSloped = isNowSloped;
    }

    void CheckCeiling()
    {
        RaycastHit hitInfo;
        float checkDistance = (controller.height * 0.5f + controller.skinWidth + groundCheckDistance);
        bool isNowCeiled = Physics.Raycast(
            transform.position + controller.center,
            Vector3.up, out hitInfo, checkDistance, groundLayers);
        if (isNowCeiled && !isCeiled)
            OnCeiled();
        isCeiled = isNowCeiled;

        #if UNITY_EDITOR
            Debug.DrawLine(transform.position + controller.center, transform.position + controller.center + Vector3.up * checkDistance, Color.blue);
        #endif
    }

    void CheckEdge() {
        RaycastHit hit;
        Vector3 castSource = transform.position - Vector3.up * (controller.skinWidth + controller.height * 0.5f);
        float castDistance = controller.radius + slipCheckDistance;
        if (Physics.Raycast(castSource, forwardSource.forward, out hit, castDistance, groundLayers)){
            FixSlip(hit.normal);
        }
        if (Physics.Raycast(castSource, -forwardSource.forward, out hit, castDistance, groundLayers)
            || Physics.Raycast(castSource, forwardSource.right, out hit, castDistance, groundLayers)
            || Physics.Raycast(castSource, -forwardSource.right, out hit, castDistance, groundLayers))
        {
            FixSlip(hit.normal);
        }
    }

    void FixSlip(Vector3 direction) {
        controller.Move((direction * slipSpeed + Vector3.down * 0.5f) * Time.deltaTime);
    }

    void HorizontalMove()
    {
        float moveSmooth = isGrounded ? groundAcceleration : airAcceleration;

        Vector3 targetHorizontalVelocity = (forwardSource.right * inputAxis.x + forwardSource.forward * inputAxis.y) * walkSpeed;
        smoothHorizontalVelocity = Vector3.Lerp(smoothHorizontalVelocity, targetHorizontalVelocity, Time.deltaTime * moveSmooth);
        horizontalVelocity = smoothHorizontalVelocity;
    }

    void VerticalMove()
    {
        if (inputJump)
        {
            if (IsGrounded)
                Jump();
            else
                jumpQueueTimer = jumpQueueTime;
            inputJump = false;
        }

        if (isGrounded)
             verticalVelocity = Vector3.zero;

        float verticalSpeed = Vector3.Dot(gravityDirection, controller.velocity);
        float gravityMultiplier = 1.0f;
        if (verticalSpeed > 0f)
            gravityMultiplier += fallMultiplier;
        else if (verticalSpeed < 0f && !inputHoldJump)
            gravityMultiplier += jumpMultiplier;
        verticalVelocity += gravityDirection * gravity * gravityMultiplier * Time.deltaTime;
    }

    void Move() 
    {
        if (isGrounded) {
            // Project horizontal velocity onto ground plane
            var projectedVelocity = Vector3.ProjectOnPlane(horizontalVelocity, groundNormal);
            horizontalVelocity = projectedVelocity.normalized * horizontalVelocity.magnitude;
            // Fix vertical slope movement
            if (horizontalVelocity.y > 0f)
                horizontalVelocity -= verticalVelocity;
        }else if (isSloped && Vector3.Angle(Vector3.up, slopeNormal) > controller.slopeLimit) {
            // Sliding movement
            horizontalVelocity *= slideVelocityMultiplier;
            var projectedVerticalVelocity = Vector3.ProjectOnPlane(verticalVelocity, slopeNormal);
            verticalVelocity = projectedVerticalVelocity;
        }

        if (!isGrounded && controller.velocity.y < 0) {
            print("Checking slip");
            CheckEdge();
        }

        // Move using computed velocities
        controller.Move((horizontalVelocity + verticalVelocity) * Time.deltaTime);
    }

    void Jump()
    {
        // Move up by ground check distance to avoid hitting ground next frame (if jump velocity is too low)
        controller.Move(Vector3.up * groundCheckDistance);
        // Compute the vertical velocity needed to reach jump height
        verticalVelocity = Vector3.up * Mathf.Sqrt(2f * jumpHeight * gravity);
        // Callback
        OnJumped();
    }

    // Callbacks
    protected virtual void OnJumped()
    {
        // Reset timer because we jumped manually
        coyoteJumpTimer = coyoteJumpTime;
        // Reset isGrounded flag
        isGrounded = false;
        isSloped = false;
        groundNormal = Vector3.up;
    }

    protected virtual void OnLanded()
    {
        // Project current velocity onto the ground plane
        var projectedVelocity = Vector3.ProjectOnPlane(verticalVelocity, groundNormal);
        verticalVelocity = projectedVelocity.normalized * verticalVelocity.magnitude;

        // Reset jump timers
        coyoteJumpTimer = 0.0f;
        if (jumpQueueTimer > 0.0f)
        {
            inputJump = true;
            jumpQueueTimer = 0.0f;
        }
    }

    protected virtual void OnCeiled() {
        // Reset velocity when touching ceiling
        verticalVelocity = Vector3.zero;
    }
}