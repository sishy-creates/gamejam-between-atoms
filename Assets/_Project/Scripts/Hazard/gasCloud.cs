using Unity.VisualScripting;
using UnityEngine;

public class gasCloud : MonoBehaviour
{
    GameObject player;
    float playerSpeed;
    float newSpeed;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player = collision.GameObject();
            //store original player speed
            //decrement player speed until OnExit was called
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //ripristina la velocità originale
        }
    }
}
