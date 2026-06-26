using UnityEngine;

using UnityEngine;

public class ChargerParticle : MonoBehaviour
{
    [Header("Visual body")]
    [SerializeField] private Transform body;
    [SerializeField] private float amplitude = 0.4f;
    [SerializeField] private float frequency = 1f;

    [Header("Magnetic field")]
    [SerializeField] private CircleCollider2D attractionArea;
    [SerializeField] private float attractionForce = 50f;

    public bool charge = true;

    private Vector3 bodyStartLocalPosition;

    private void Awake()
    {
        if (body == null)
        {
            body = transform.parent.Find("Circle");
        }

        if (attractionArea == null)
        {
            attractionArea = GetComponent<CircleCollider2D>();
        }

        if (body == null)
        {
            Debug.LogError("ChargerParticle: Body/Circle non trovato.", this);
            enabled = false;
            return;
        }

        if (attractionArea == null)
        {
            Debug.LogError(
                "ChargerParticle: CircleCollider2D non trovato.",
                this
            );

            enabled = false;
            return;
        }

        bodyStartLocalPosition = body.localPosition;
        attractionArea.isTrigger = true;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(2f * Mathf.PI * frequency * Time.time) * amplitude;

        body.localPosition = bodyStartLocalPosition + Vector3.up * offsetY;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D playerRb = other.attachedRigidbody;

        if (playerRb == null || !playerRb.CompareTag("Player"))
        {
            return;
        }

        
        Vector2 fieldCenter = attractionArea.bounds.center;

        Vector2 toCenter = fieldCenter - playerRb.position;

        float distance = toCenter.magnitude;

        if (distance < 0.001f)
        {
            return;
        }

        float worldRadius = attractionArea.bounds.extents.x;

        float distance01 = Mathf.Clamp01(distance / worldRadius);

        float localForce = (1f - distance01)*0.5f;

        if (charge)
        {
            playerRb.AddForce(toCenter.normalized * attractionForce * localForce, ForceMode2D.Force);
        }
        else
        {
            playerRb.AddForce(-toCenter.normalized * attractionForce * localForce, ForceMode2D.Force);
        }
    }
}
