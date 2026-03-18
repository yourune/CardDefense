using UnityEngine;

/// <summary>
/// Tornado zone that damages enemies and pulls them toward center.
/// Special Effect: Enemies are pulled toward center and cannot move.
/// </summary>
public class TornadoZone : EffectZone
{
    // Damage settings configured in Inspector through base class:
    // - damagePerTick = 3
    // - damageTickInterval = 1f
    
    // Special Effect: Pull enemies (Not implemented yet)
    [Header("Tornado Special Effect")]
    [SerializeField] private float pullStrength = 5f;
    
    protected override void UpdateZone()
    {
        // TODO: Implement pull effect
        // Pull enemies toward center
        // Disable enemy movement while in tornado
    }
}
