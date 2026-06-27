using UnityEngine;
using UnityEngine.Events;

public class collapsingPlatform : MonoBehaviour
{
    [SerializeField] private float collapsingTime = 1.4f;
    [SerializeField] private UnityEvent onCollapse;


    private bool collaps = false;
    float index = 0; 

    private void Awake()
    {
        collapsingTime = 1.4f;
    }

    private void Update()
    {
        if (collaps)
        {
            index += Time.deltaTime;
            if (index >= collapsingTime)
            {
                onCollapse.Invoke();
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
}
