using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float maxMoveSpeed;
    public float sprintSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField] private float skyDrag;
    public float groundDrag;
    bool grounded;
    public float fadeDuration = 1f;

    [SerializeField] private float grapSpeed, maxGrapSpeed;
    [SerializeField] private float jumpSpeed;


    public PlayerInputSystem inputSystem;
    public Rigidbody rb;
    Grappling grappling;

    Transform playerCamera;

    public Vector2 inputDirection;

    public bool onJump = false;

    public bool freeze;

    public bool activeGrapple;

    public bool shotButtonState = false;

    private void Awake()
    {
        inputSystem = new PlayerInputSystem();
        rb = GetComponent<Rigidbody>();
        grappling = GetComponent<Grappling>();
    }

    private void Start()
    {
        readyToJump = true;
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = activeGrapple ? 0 : skyDrag;
        }
    }

    public void Release()
    {
        rb.useGravity = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        activeGrapple = false;
    }

    public void Shot()
    {
        rb.velocity = Vector3.zero;
        activeGrapple = true;
    }

    public void Move(Vector2 dir)
    {
        Vector3 moveDirection = playerCamera.forward * dir.y + playerCamera.right * dir.x;
        moveDirection.y = 0;

        SpeedControl();

        if (activeGrapple) return;

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (activeGrapple) return;

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > maxMoveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * maxMoveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition)
    {
        activeGrapple = true;

        rb.useGravity = false;

        var velocityToSet = CalculateJumpVelocity(transform.position, targetPosition);
        var velocityToSetNormal = velocityToSet.normalized;
        enableMovementOnNextTouch = true;
        if (rb.velocity.magnitude > maxGrapSpeed) return;
        rb.AddForce(new Vector3(velocityToSetNormal.x * grapSpeed + velocityToSet.x, velocityToSetNormal.y * grapSpeed + velocityToSet.y, velocityToSetNormal.z * grapSpeed + velocityToSet.z));
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint)
    {
        float gravity = Physics.gravity.y;
        Vector3 def = endPoint - startPoint;

        return new Vector3(def.x, def.y + Mathf.Sqrt(-2 * gravity), def.z);
    }
}