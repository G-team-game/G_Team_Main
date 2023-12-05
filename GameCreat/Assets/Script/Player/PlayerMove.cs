using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundDrag;
    bool grounded;

    [Header("Ready to Start")]
    public Image fadeImage;
    public float fadeDuration = 1f;


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
        rb= GetComponent<Rigidbody>();
        grappling= GetComponent<Grappling>();
    }

    private void Start()
    {
        fadeImage.enabled = true;

        readyToJump = true;

        playerCamera = Camera.main.transform;

        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        inputSystem.Player.Fire.started += ctx => shotButtonState = true;
        inputSystem.Player.Fire.canceled += ctx => shotButtonState = false;

        inputSystem.Player.Jump.started += PlayerJump;
        inputSystem.Player.GrapplingShot.started += PlayerGrappling;

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Fire.started -= ctx => shotButtonState = true;
        inputSystem.Player.Fire.canceled -= ctx => shotButtonState = false;

        inputSystem.Player.Jump.started -= PlayerJump;
        inputSystem.Player.GrapplingShot.started -= PlayerGrappling;

        inputSystem.Disable();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        inputDirection = inputSystem.Player.Move.ReadValue<Vector2>();

        SpeedControl();

        if (grounded && !activeGrapple)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDirection = playerCamera.forward * inputDirection.y + playerCamera.right * inputDirection.x;

        moveDirection.y = 0;

        if (activeGrapple) return;

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    void PlayerJump(InputAction.CallbackContext obj)
    {
        if (readyToJump)
        {
        readyToJump = false;

        Jump();

        Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void PlayerGrappling(InputAction.CallbackContext obj)
    {
        grappling.PlayerShot();
    }

    private void SpeedControl()
    {
        if (activeGrapple) return;

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
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

            GetComponent<Grappling>().StopGrapple();
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    IEnumerator FadeIn()
    {
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            fadeImage.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), t / fadeDuration);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);

        fadeImage.enabled = false;
    }
}
