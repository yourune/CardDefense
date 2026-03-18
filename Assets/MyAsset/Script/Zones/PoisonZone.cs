using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Poison zone that damages enemies over time.
/// Special Effect: Poison stack accumulates the longer enemies stay.
/// Stack increases poison damage over time.
/// </summary>
public class PoisonZone : EffectZone
{
    // Damage settings configured in Inspector through base class:
    // - damagePerTick = 2
    // - damageTickInterval = 0.5f
    
    // Special Effect: Poison Stack (Not implemented yet)
    private Dictionary<EnemyView, int> poisonStacks = new Dictionary<EnemyView, int>();
    
    protected override void OnEnemyEnter(EnemyView enemy)
    {
        // Initialize poison stack
        poisonStacks[enemy] = 0;
    }
    
    protected override void OnEnemyStay(EnemyView enemy)
    {
        // TODO: Implement poison stack accumulation
        // Each tick increases stack, stack multiplies damage
    }
    
    protected override void OnEnemyExit(EnemyView enemy)
    {
        // Clear poison stack when leaving
        poisonStacks.Remove(enemy);
    }
}
