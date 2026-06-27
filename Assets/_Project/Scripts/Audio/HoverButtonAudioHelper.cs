using UnityEngine;
using UnityEngine.EventSystems; 
using FMODUnity;
using FMOD.Studio;
public class HoverButtonAudioHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private EventReference onHoverEvent;
    [SerializeField] private EventReference onClickEvent;
    
    public void PlaySelectSound()
    {
        if (!onClickEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(onClickEvent);
        }      
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!onHoverEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(onHoverEvent);
        }        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }
}