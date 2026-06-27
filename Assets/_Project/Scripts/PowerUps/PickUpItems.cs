using UnityEngine;

public class PickupItem : MonoBehaviour
{
    
    public enum ItemType
    {
        // Jump +
        GreenPowerUp_JumpUpgrade,
        // Speed +
        YellowPowerUp_SpeedBoost,
        // Dash
        Blue_PowerUp
    }

    [Header("Item Configuration")]
    public ItemType type;
    public float duration = 5f;        
    public float intensityMultiplier = 1.5f; 
}