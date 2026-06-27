using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private EventReference footStepsSoundEvent;
    [SerializeField] private EventReference jumpSoundEvent;
    [SerializeField] private EventReference deathSoundEvent;
    [SerializeField] private EventReference slipSoundEvent;
    [SerializeField] private EventReference impactSoundEvent;
    [SerializeField] private EventReference pickupSoundEvent;

    // 0 = Table - 1 = Garden - 2 = Atomic Dimension
    private int currentSurfaceType = 0;

    private EventInstance footstepInstance;

    void Start()
    {
        if (footStepsSoundEvent.IsNull) return;
        footstepInstance = RuntimeManager.CreateInstance(footStepsSoundEvent);
    }

    public void PlayFootstepFromAnimation()
    {
        if (footStepsSoundEvent.IsNull) return;
        footstepInstance.setParameterByName("Surface", currentSurfaceType);
        footstepInstance.start();
    }

    public void PlayJumpSound()
    {
        if (!jumpSoundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(jumpSoundEvent, transform.position);
        }
    }

    public void PlayDeathSound()
    {
        if (!deathSoundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(deathSoundEvent, transform.position);
        }
    }

    public void PlaySlipSound()
    {
        if (!slipSoundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(slipSoundEvent, transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            return;
        }

        if (!impactSoundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(impactSoundEvent);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Ground"))
        {
            return;
        }
        
        if (other.CompareTag("TODO"))
        {
            if (!pickupSoundEvent.IsNull)
            {
                RuntimeManager.PlayOneShot(pickupSoundEvent, other.transform.position);
            }

        }


        if (other.CompareTag("TableZone"))
        {
            currentSurfaceType = 0;
        }
        else if (other.CompareTag("GardenZone"))
        {
            currentSurfaceType = 1;
        }
        else if (other.CompareTag("AtomicZone"))
        {
            currentSurfaceType = 2;
        }

    }

    void OnDestroy()
    {
        if (footstepInstance.isValid())
        {
            footstepInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            footstepInstance.release();
        }
    }

}