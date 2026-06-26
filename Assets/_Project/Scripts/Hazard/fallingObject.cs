using UnityEngine;

public class fallingObject : MonoBehaviour
{
    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void StartFalling()
    {
        m_rb.gravityScale = 2.0f;
    }
}
