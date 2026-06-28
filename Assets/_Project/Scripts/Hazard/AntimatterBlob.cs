using UnityEngine;

public class AntimatterBlob : MonoBehaviour
{
    CircleCollider2D m_circleCollider;

    Transform player;
    
    Rigidbody2D p_rb;
    
    PlayerController playerController;
    
    Vector3 playerScale;
    
    private float originalGravityScale;

    [SerializeField] private float movemetRadius = 0.5f;
    [SerializeField] private float movementSpeed = 1f;

    private float seedX;
    private float seedY;
    
    [SerializeField] private bool antiMatterCondition = false; 
    [SerializeField] private float counter;
    [SerializeField] private float antiMatterTime;
    [SerializeField] private bool alreadyUsed;

    Vector3 startPosition;

    private Animator m_animator;

    void Awake()
    {
        m_circleCollider = GetComponent<CircleCollider2D>();
        m_circleCollider.isTrigger = true;

        startPosition = transform.position;

        seedX = Random.Range(0f, 1000f);
        seedY = Random.Range(0f, 1000f);

        movementSpeed = 0.8f;
        movemetRadius = 1;

        alreadyUsed = false;
        antiMatterCondition = false;
        antiMatterTime = 5f;
        counter = 0f;

        m_animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        float time = Time.time * movementSpeed;

        float x = Mathf.PerlinNoise(seedX, time) * 2f - 1f;
        float y = Mathf.PerlinNoise(seedY, time) * 2f - 1f;

        Vector2 movement = new Vector2(x, y);
        movement = Vector2.ClampMagnitude(movement, 1f);
        transform.localPosition = startPosition + (Vector3)(movement * movemetRadius);


        if (antiMatterCondition)
        {
            counter += Time.deltaTime;
        }

        if(counter >= antiMatterTime)
        {
            Debug.Log("Anti-matter condition end");

            antiMatterCondition = false;
            player.transform.Rotate(0, 0, -180);

            playerScale = new Vector3(playerScale.x * -1, playerScale.y, playerScale.z);
            player.localScale = playerScale;

            playerController.controlsInverted = false;

            p_rb.gravityScale = originalGravityScale;
            counter = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Player"))
            return;

        if (alreadyUsed)
            return;

        player = collision.transform;
        playerController = player.transform.GetComponent<PlayerController>();
        p_rb = collision.transform.GetComponent<Rigidbody2D>();

        Debug.Log("Start anti-matter condition");

        originalGravityScale = p_rb.gravityScale;
        p_rb.gravityScale = originalGravityScale * -1;

        collision.transform.Rotate(0, 0, -180);
        playerScale = player.transform.localScale;
        playerScale = new Vector3(playerScale.x * -1 ,playerScale.y ,playerScale.z);
        player.localScale = playerScale;

        playerController.controlsInverted = true;
           
        antiMatterCondition = true;
        alreadyUsed = true;
    }
}
