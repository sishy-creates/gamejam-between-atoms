using UnityEngine;

public class DogController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.2f;

    [Header("Gap Check")]
    [SerializeField] private Transform gapCheck;
    [SerializeField] private float gapCheckDistance = 0.6f;

    [Header("Enemy Check")]
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isCaught;
    private bool hasJumpedOverObstacle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
{
    if (isCaught) return;

    MoveForward();

    bool obstacleDetected = CheckWall() || CheckGap();

    if (obstacleDetected && !hasJumpedOverObstacle)
    {
        Jump();
        hasJumpedOverObstacle = true;
    }

    if (!obstacleDetected && isGrounded)
    {
        hasJumpedOverObstacle = false;
    }
}

    private void MoveForward()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private bool CheckWall()
{
    LayerMask obstacleLayers = groundLayer | enemyLayer;

    RaycastHit2D hit = Physics2D.Raycast(
        wallCheck.position,
        Vector2.right,
        wallCheckDistance,
        obstacleLayers
    );

    return hit.collider != null;
}

    private bool CheckGap()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            gapCheck.position,
            Vector2.down,
            gapCheckDistance,
            groundLayer
        );

        return hit.collider == null;
    }

   private void Jump()
{
    if (!isGrounded) return;

    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
}

    public void StopDog()
    {
        isCaught = true;
        rb.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                wallCheck.position,
                wallCheck.position + Vector3.right * wallCheckDistance
            );
        }

        if (gapCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                gapCheck.position,
                gapCheck.position + Vector3.down * gapCheckDistance
            );
        }
    }
}