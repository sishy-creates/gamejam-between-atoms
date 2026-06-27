using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SimpleAudioController : MonoBehaviour
{
    [SerializeField] private EventReference soundEvent;
    [SerializeField] private EventReference oneShotEvent;
    [SerializeField] private EventReference oneShotEvent2;
    [SerializeField] private EventReference oneShotEvent3;
    [SerializeField] private EventReference collisionSoundEvent;
    [SerializeField] private EventReference triggerSoundEvent;

    private EventInstance instance;


    public void PlayOneShot()
    {
         if (!oneShotEvent.IsNull)
            {
                RuntimeManager.PlayOneShot(oneShotEvent);
            }
    }
    public void PlayOneShot2()
    {
         if (!oneShotEvent2.IsNull)
            {
                RuntimeManager.PlayOneShot(oneShotEvent2);
            }
    }
    public void PlayOneShot3()
    {
         if (!oneShotEvent3.IsNull)
            {
                RuntimeManager.PlayOneShot(oneShotEvent3);
            }
    }

    void Start()
    {
        if (!soundEvent.IsNull)
        {
            instance = RuntimeManager.CreateInstance(soundEvent);
            RuntimeManager.AttachInstanceToGameObject(instance, transform);
            instance.start();
            Debug.Log("Event Started");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!collisionSoundEvent.IsNull)
        {
                Vector3 contactPoint = collision.contacts[0].point;
                RuntimeManager.PlayOneShot(collisionSoundEvent, contactPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerSoundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(triggerSoundEvent, other.transform.position);
        }
    }

    void OnDestroy()
    {
        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }


}
