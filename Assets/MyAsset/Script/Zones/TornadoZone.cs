using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tornado zone that pulls enemies toward center
/// </summary>
public class TornadoZone : EffectZone
{
    [SerializeField] private float pullStrength = 5f;
    [SerializeField] private int damagePerSecond = 3;
    [SerializeField] private GameObject tornadoVFX;
    
    private float damageTimer;
    
    protected override void OnZoneCreated()
    {
        if (tornadoVFX != null)
        {
            Instantiate(tornadoVFX, transform.position, Quaternion.identity, transform);
        }
    }
    
    protected override void UpdateZone()
    {
        damageTimer += Time.deltaTime;
        
        if (damageTimer >= 1f)
        {
            damageTimer = 0f;
            
            // Collect enemies to remove to avoid modifying collection during iteration
            System.Collections.Generic.List<EnemyView> enemiesToRemove = new System.Collections.Generic.List<EnemyView>();
            
            // Damage enemies in tornado - apply directly without ActionSystem
            foreach (var enemy in enemiesInZone)
            {
                if (enemy != null && enemy.currentHealth > 0)
                {
                    enemy.TakeDamage(damagePerSecond);
                    
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
        
        // Pull enemies toward center
        foreach (var enemy in enemiesInZone)
        {
            if (enemy != null)
            {
                Vector3 direction = (transform.position - enemy.transform.position).normalized;
                enemy.transform.position += direction * pullStrength * Time.deltaTime;
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
        Debug.Log($"{enemy.name} caught in tornado");
    }
    
    protected override void OnEnemyStay(EnemyView enemy)
    {
        // Pulling handled in UpdateZone
    }
    
    protected override void OnEnemyExit(EnemyView enemy)
    {
        Debug.Log($"{enemy.name} escaped tornado");
    }
    
    protected override void OnZoneExpired()
    {
        Debug.Log("Tornado dissipated");
    }
}
