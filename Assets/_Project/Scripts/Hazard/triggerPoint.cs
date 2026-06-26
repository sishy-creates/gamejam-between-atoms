using UnityEngine;

public class triggerPoint : MonoBehaviour
{
    private Transform m_body;
    private fallingObject fallingSystem;

    private void Awake()
    {
        m_body = transform.parent.Find("Body");
        if (m_body != null)
        {
            fallingSystem = m_body.GetComponent<fallingObject>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")){
            Debug.Log("Player detected");
            fallingSystem.StartFalling();
        }
    }
}
