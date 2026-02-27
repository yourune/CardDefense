using UnityEngine;

public class Interactions : Singleton<Interactions>
{
    public bool PlayerIsDragging { get; set; } = false;
    
    private float lastCardUsedTime = -1f;
    private const float cardUseCooldown = 0.2f;

    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        return false;
    }
    
    public bool PlayerCanHover()
    {
        if(PlayerIsDragging) return false;
        if(Time.time < lastCardUsedTime + cardUseCooldown) return false;
        return true;
    }
    
    public void SetCardUsedCooldown()
    {
        lastCardUsedTime = Time.time;
    }
}
