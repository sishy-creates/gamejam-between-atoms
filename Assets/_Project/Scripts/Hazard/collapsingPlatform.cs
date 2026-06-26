using UnityEngine;

public class collapsingPlatform : MonoBehaviour
{
    bool collaps = false;
    float collapsingTime;
    float index = 0; 

    private void Awake()
    {
        collapsingTime = 3f;
    }

    private void Update()
    {
        if (collaps)
        {
            index += Time.deltaTime;
            if (index >= collapsingTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collaps = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collaps = false;
            index = 0;
        }
    }
}
