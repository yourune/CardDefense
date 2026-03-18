using UnityEngine;

/// <summary>
/// Rain zone that creates wet ground.
/// Special Effects: 
/// 1. Creates water puddles on the ground
/// 2. Enemies that step on wet ground are slowed
/// </summary>
public class RainZone : EffectZone
{
    // Damage settings configured in Inspector through base class:
    // - damagePerTick = 0 (no damage)
    // - damageTickInterval = N/A
    
    // Special Effect: Wet ground and slow (Not implemented yet)
    [Header("Rain Special Effects")]
    [SerializeField] private float slowPercentage = 30f; // 30% slow
    
    protected override void UpdateZone()
    {
        // TODO: Implement wet ground creation
        // Create water puddles in grid pattern
        // ZoneManager.Instance.SetGroundState(pos, GroundStateType.Wet, duration)
    }
    
    protected override void OnEnemyEnter(EnemyView enemy)
    {
        // TODO: Apply slow debuff
        // Reduce enemy.Speed by slowPercentage
    }
    
    protected override void OnEnemyExit(EnemyView enemy)
    {
        // TODO: Remove slow debuff
        // Restore enemy.Speed
    }
}
