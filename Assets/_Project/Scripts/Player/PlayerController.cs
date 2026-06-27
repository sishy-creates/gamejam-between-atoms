using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    //Events
    [Header("Events")]
    [SerializeField] private UnityEvent onSlide;

    [Header("Movement")]
    [SerializeField] public float walkSpeed = 3f;
    [SerializeField] public float runSpeed = 6f;
    [SerializeField] private float acceleration = 50f;

    [Header("Jump")]
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private int maxJumps = 2;

    [Header("Wall Jump and Slide")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 10f);
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.2f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash (GDD Feature)")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Status Modifiers (GDD Alterations)")]
    public float speedMultiplier = 1f;
    public float jumpMultiplier = 1f;
    public bool controlsInverted = false;
    public bool hasDashUnlocked = false; // Unlocked via Neon-Blue potion

    private Rigidbody2D rb;
    private float horizontalInput;
    private float currentBaseSpeed;
    private float wallCheckDelayTimer;
    private int jumpsLeft;

    private bool isGrounded;
    private bool isWalled;
    private bool isWallSliding;
    private bool isFacingRight = true;
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
    }

    private void Update()
    {
        // Block player inputs during a dash
        if (isDashing) return;

        HandleInput();
        HandleFlip();

        // Jump inputs handling (Normal, Double and Wall Jump)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isWallSliding)
            {
                WallJump();
            }
            else if (jumpsLeft > 0)
            {
                Jump();
            }
        }

        // Variable jump height: cuts vertical velocity short when spacebar is released early
        if (Keyboard.current.spaceKey.wasReleasedThisFrame && rb.linearVelocity.y * transform.localScale.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // Dash trigger condition
        if (Keyboard.current.fKey.wasPressedThisFrame && canDash && hasDashUnlocked && horizontalInput != 0)
        {
            StartCoroutine(PerformDash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        // Environment overlap checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Prevents instant re-sticking to walls right after a wall jump
        if (wallCheckDelayTimer > 0)
        {
            wallCheckDelayTimer -= Time.fixedDeltaTime;
            isWalled = false;
        }
        else
        {
            isWalled = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
        }

        // Reset jump counters when touching the ground safely
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpsLeft = maxJumps;
        }

        HandleWallSlide();
        Move();
    }

    private void HandleInput()
    {
        // Sprint check: Left Shift toggles run speed if grounded
        if (Keyboard.current.leftShiftKey.isPressed && isGrounded)
        {
            currentBaseSpeed = runSpeed;
        }
        else if (!isGrounded)
        {
            // Keeps current sprint/walk speed state mid-air to prevent mid-flight friction
        }
        else
        {
            currentBaseSpeed = walkSpeed;
        }

        // Direct raw input listening
        float rawInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) rawInput = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) rawInput = 1f;

        // Stage 3 Antimateria effect: flips controls dynamically if true
        horizontalInput = controlsInverted ? -rawInput : rawInput;
    }

    private void Move()
    {
        float targetSpeed = horizontalInput * (currentBaseSpeed * speedMultiplier);
        float currentAcc = acceleration;

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, currentAcc * Time.fixedDeltaTime);

        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(newX, -wallSlideSpeed * Mathf.Sign(transform.localScale.y));
        }
        else
        {
            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
    }

    private void Jump()
    {
        // Multiplier hooks into the green potion status
        float finalJumpForce = jumpForce * jumpMultiplier;

        // Normal vs inverted gravity
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, finalJumpForce * Mathf.Sign(transform.localScale.y));
        jumpsLeft--;
    }

    private void WallJump()
    {
        float jumpDirection = isFacingRight ? -1f : 1f;

        rb.linearVelocity = new Vector2(jumpDirection * wallJumpForce.x, wallJumpForce.y * Mathf.Sign(transform.localScale.y));

        // Consumes first jump slot 1 extra remaining
        jumpsLeft = maxJumps - 1;

        wallCheckDelayTimer = 0.25f;
        isWallSliding = false;

        Flip();
    }

    private void HandleWallSlide()
    {
        bool pressingToWall = (isFacingRight && horizontalInput > 0) || (!isFacingRight && horizontalInput < 0);

        if (isWalled && pressingToWall && !isGrounded && rb.linearVelocity.y <= 0)
        {
            isWallSliding = true;
            jumpsLeft = maxJumps;
            onSlide.Invoke();
        }
        else
        {
            isWallSliding = false;
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        // Stores active gravity scaling
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Horizontal vector push
        rb.linearVelocity = new Vector2(horizontalInput * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        // Reset gravity and physics state back to baseline
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void HandleFlip()
    {
        if (isWallSliding || horizontalInput == 0) return;

        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            Flip();
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null) Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        if (wallCheck != null) Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }

    public void Die()
{
    RespawnManager.Instance.Respawn();
}

public void ResetPlayerState()
{
    rb.linearVelocity = Vector2.zero;

    horizontalInput = 0f;
    currentBaseSpeed = walkSpeed;
    wallCheckDelayTimer = 0f;
    jumpsLeft = maxJumps;

    isWallSliding = false;
    isDashing = false;
    canDash = true;
}
}