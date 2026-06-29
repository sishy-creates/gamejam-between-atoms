using UnityEngine;
using UnityEngine.Events;

public class rollingSystem : MonoBehaviour
{
    // You can keep this visible in the Inspector to watch it turn on and off while testing!
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rollingSpeed = 3f;

    [SerializeField] private UnityEvent onTouchGround;

    private Rigidbody2D m_rb;
    private Animator m_animator;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        // Prevents the physical circle from spinning
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (m_animator != null)
        {
            m_animator.SetBool("isGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            Vector3 targetPosition = transform.position + Vector3.left * Time.fixedDeltaTime * rollingSpeed;
            m_rb.MovePosition(targetPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Tube hit Player");
            GameManager.Instance.GameOver();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Tube touches the ground -> Start rolling

            if (!isGrounded)
            {
                onTouchGround.Invoke();
            }

            isGrounded = true;
        }
    }

    // --- NEW METHOD ---
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Tube slips off the ground -> Start falling
            isGrounded = false;
        }
    }
}