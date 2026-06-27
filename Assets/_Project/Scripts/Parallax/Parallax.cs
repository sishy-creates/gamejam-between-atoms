using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;

    public Camera cam;
    public Transform subject;

    // The relative distance between the object and the subject
    float distanceFromSubject => transform.position.z - subject.position.z;

    float parallaxEffect
    {
        get
        {
            if (distanceFromSubject > 0)
            {
                // Find the absolute World Z of the far plane
                float farPlaneWorldZ = cam.transform.position.z + cam.farClipPlane;

                // Find the relative distance from the subject to that far plane
                float distanceToFarPlane = farPlaneWorldZ - subject.position.z;

                // Divide and Clamp. (Clamp01 strictly prevents the value from going over 1.0, which stops far objects from moving faster than the camera).
                return distanceToFarPlane > 0 ? Mathf.Clamp01(distanceFromSubject / distanceToFarPlane) : 0;
            }
            else
            {
                // Find the absolute World Z of the near plane
                float nearPlaneWorldZ = cam.transform.position.z + cam.nearClipPlane;

                // Find the relative distance from the near plane to the subject
                float distanceToNearPlane = subject.position.z - nearPlaneWorldZ;

                // Divide. (distanceFromSubject is negative, so this correctly returns a negative parallax effect, making foregrounds zip past the camera).
                return distanceToNearPlane > 0 ? (distanceFromSubject / distanceToNearPlane) : 0;
            }
        }
    }

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        // 0 == move with player || 1 == completely stationary background || negative == fast foreground
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}