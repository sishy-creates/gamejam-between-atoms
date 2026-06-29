using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Events;


public class DogPope : MonoBehaviour
{
    [SerializeField] private float incapacityTime = 3f;
    [SerializeField] private bool p_incapacity = false;

    [SerializeField] private UnityEvent onHit;


    private float originalJumpForce;
    private float originalWalkSpeed;
    private float originalRunSpeed;

    [SerializeField] private float count = 0;

    PlayerController playerController;

    private void Awake()
    {
        count = 0;
        incapacityTime = 3f;
        p_incapacity = false;
    }

    private void Update()
    {

        if (p_incapacity)
        {
            Debug.Log("Il player è incapacitato");
            count += Time.deltaTime;
        }

        if (count >= incapacityTime)
        {
            Debug.Log("Il player non è più incapacitato");
            p_incapacity = false;
            count = 0;

            playerController.walkSpeed = originalWalkSpeed;
            playerController.runSpeed = originalRunSpeed;
            playerController.jumpForce = originalJumpForce;

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Player"))
            return;

        Debug.Log("Start slow down animation");

        playerController = collision.GetComponent<PlayerController>();

        originalWalkSpeed = playerController.walkSpeed;
        originalRunSpeed = playerController.runSpeed;
        originalJumpForce = playerController.jumpForce;

        playerController.walkSpeed = 0f;
        playerController.runSpeed = 0f;
        playerController.jumpForce = 0f;

        p_incapacity = true;

        onHit.Invoke();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
}
