using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomCharacterController2 : MonoBehaviour
{
    #pragma warning disable 649, 414

    // Parameters
    [Header("Walk Settings")]
    [SerializeField] float walkSpeed = 10.0f;
    [SerializeField] float groundAcceleration = 24.0f;
    [SerializeField] float airAcceleration = 4.0f;

    [Header("Gravity Settings")]
    [SerializeField] float gravity = 30.0f;
    [SerializeField] float maxVelocity = 100f;
    [SerializeField] Vector3 gravityDirection;

    [Header("Other Settings")]
    [SerializeField] CollisionChecker groundChecker;

    // Components
    Rigidbody rigidbody;
    
    // Input
    private Vector3 moveAxis;

    // Variables
    Vector3 horizontalVelocity;
    Quaternion targetRotation;
    
    bool isGrounded;
    float jumpQueueTimer;
    float coyoteJumpTimer;
    bool jumpQueued;

    void Awake()
    {
        // Fetch references
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetMoveAxis(Vector2 axis) => moveAxis = axis;

    void FixedUpdate(){
        // Ground checking
        CheckGround();

         // Horizontal movement
        HorizontalMove();

        // Vertical movement
        VerticalMove();
        
        if (rigidbody.velocity.magnitude > maxVelocity)
            rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;
    }

    void HorizontalMove(){
        float moveSmooth = isGrounded ? groundAcceleration : airAcceleration;

        Vector3 targetHorizontalVelocity = (transform.right * moveAxis.x + transform.forward * moveAxis.y) * walkSpeed;
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetHorizontalVelocity, moveSmooth * Time.fixedDeltaTime);
        //rigidbody.AddForce(horizontalVelocity, ForceMode.VelocityChange);
        rigidbody.MovePosition(rigidbody.position + horizontalVelocity * Time.fixedDeltaTime);
    }

    void VerticalMove(){
        float gravityMultiplier = 1.0f;

        rigidbody.AddForce(gravityDirection * gravity * gravityMultiplier, ForceMode.Force);     
    }

    void CheckGround(){
        bool isNowGrounded = groundChecker.isColliding();

        if (isNowGrounded && !isGrounded)
            OnLanded();
        isGrounded = isNowGrounded;
    }

    // Callbacks
    void OnLanded(){
        
    }
}
