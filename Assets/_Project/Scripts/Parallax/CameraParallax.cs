using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + 0.001f, transform.position.y, transform.position.z);
    }
}
