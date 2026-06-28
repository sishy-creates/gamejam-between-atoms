using UnityEngine;

public class triggerPoint : MonoBehaviour
{
    private Transform m_body;
    private fallingPotion fallingPotion;

    private void Awake()
    {
        m_body = transform.parent.Find("Body");
        if (m_body != null)
        {
            fallingPotion = m_body.GetComponent<fallingPotion>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")){
            Debug.Log("Player detected");
            fallingPotion.StartFalling();
        }
    }
}
