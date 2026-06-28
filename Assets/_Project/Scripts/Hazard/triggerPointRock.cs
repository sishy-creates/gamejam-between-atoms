using UnityEngine;

public class triggerPointRock: MonoBehaviour
{
    [SerializeField] private Transform m_body;
    [SerializeField] private fallingRock fallingRock;

    private void Awake()
    {
        m_body = transform.parent.Find("Body");
        if (m_body != null)
        {
            fallingRock = m_body.GetComponent<fallingRock>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")){
            Debug.Log("Player detected");
            fallingRock.StartFalling();
        }
    }
}
