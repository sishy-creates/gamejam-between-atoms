using Unity.VisualScripting;
using UnityEngine;

public class gasCloud : MonoBehaviour
{
    [Header("GAS CLOUD")]

    [SerializeField] private int speedReduction;

    PlayerController playerController;
    CircleCollider2D m_circleCollider;

    float originalSpeed;

    private void Start()
    {
        m_circleCollider = transform.GetComponent<CircleCollider2D>();
        m_circleCollider.isTrigger = true;
        speedReduction = 50;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            originalSpeed = playerController.moveSpeed;
            playerController.moveSpeed = originalSpeed - ((originalSpeed / 100) * speedReduction);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerController.moveSpeed = originalSpeed;
        }
    }
}
