using System;
using UnityEngine;

public class fallingPotion : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private CircleCollider2D m_collider;

    private bool m_hasShattered = false;
    
    [SerializeField] private Animator m_animator;

    [SerializeField] private float gravityScale = 1.0f;

    [SerializeField] private GameObject gasCloud;
    [SerializeField] private Transform cloudSpawnPosition;

    private void Awake()
    {
        m_rb = transform.GetComponent<Rigidbody2D>();
        m_animator = transform.GetComponent<Animator>();
        m_collider = transform.GetComponent<CircleCollider2D>();
    }

    public void StartFalling()
    {
        m_rb.gravityScale = gravityScale;
        m_animator.SetBool("isFalling", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (m_hasShattered) return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_hasShattered = true;
            Debug.Log("Potion hit ground");
            m_animator.SetBool("isGrounded", true);
            SpawnCloud();
            Destroy(gameObject);
        } 
    }

    private void SpawnCloud()
    {
        Instantiate(gasCloud, transform.position, Quaternion.identity);
    }
}
