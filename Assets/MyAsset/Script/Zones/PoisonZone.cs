using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Poison zone that damages enemies over time and applies poison debuff
/// </summary>
public class PoisonZone : EffectZone
{
    [SerializeField] private int damagePerTick = 2;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private GameObject poisonVFX;
    
    private float nextTickTime;
    
    protected override void OnZoneCreated()
    {
        nextTickTime = Time.time + tickInterval;
        
        if (poisonVFX != null)
        {
            Instantiate(poisonVFX, transform.position, Quaternion.identity, transform);
        }
    }
    
    protected override void UpdateZone()
    {
        if (Time.time >= nextTickTime)
        {
            nextTickTime = Time.time + tickInterval;
            
            // Collect enemies to remove to avoid modifying collection during iteration
            System.Collections.Generic.List<EnemyView> enemiesToRemove = new System.Collections.Generic.List<EnemyView>();
            
            // Damage all enemies in zone - apply directly without ActionSystem
            foreach (var enemy in enemiesInZone)
            {
                if (enemy != null && enemy.currentHealth > 0)
                {
                    enemy.TakeDamage(damagePerTick);
                    
                    // Check if enemy died from this damage
                    if (enemy.currentHealth <= 0)
                    {
                        enemiesToRemove.Add(enemy);
                    }
                }
            }
            
            // Process dead enemies after iteration
            foreach (var enemy in enemiesToRemove)
            {
                HandleEnemyDeath(enemy);
            }
        }
    }
    
    private void HandleEnemyDeath(EnemyView enemy)
    {
        if (enemy == null) return;
        
        // Remove from tracking immediately
        enemiesInZone.Remove(enemy);
        
        // Drop rewards directly without ActionSystem
        if (enemy.EnemyData != null)
        {
            if (RewardVisualSystem.Instance != null)
            {
                RewardVisualSystem.Instance.SpawnRewardsDirect(
                    enemy.transform.position, 
                    enemy.EnemyData.GoldDrop, 
                    enemy.EnemyData.XpDrop
                );
            }
        }
        
        // Let EnemySystem handle cleanup directly
        if (EnemySystem.Instance != null)
        {
            EnemySystem.Instance.RemoveEnemyDirect(enemy);
        }
    }
    
    protected override void OnEnemyEnter(EnemyView enemy)
    {
        Debug.Log($"{enemy.name} entered poison zone");
        // Apply poison debuff if you have a debuff system
    }
    
    protected override void OnEnemyStay(EnemyView enemy)
    {
        // Continuous effects handled in UpdateZone
    }
    
    protected override void OnEnemyExit(EnemyView enemy)
    {
        Debug.Log($"{enemy.name} left poison zone");
    }
    
    protected override void OnZoneExpired()
    {
        Debug.Log("Poison zone expired");
    }
}
