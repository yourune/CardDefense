using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for persistent area effect zones (poison clouds, tornados, water puddles, etc.)
/// </summary>
public abstract class EffectZone : MonoBehaviour
{
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float radius = 3f;
    [SerializeField] protected LayerMask enemyLayer;
    
    protected float remainingTime;
    protected HashSet<EnemyView> enemiesInZone = new HashSet<EnemyView>();
    
    public float Radius => radius;
    public bool IsExpired => remainingTime <= 0;
    
    protected virtual void Start()
    {
        remainingTime = duration;
        OnZoneCreated();
    }
    
    protected virtual void Update()
    {
        remainingTime -= Time.deltaTime;
        
        if (remainingTime <= 0)
        {
            OnZoneExpired();
            Destroy(gameObject);
            return;
        }
        
        UpdateZone();
        CheckForEnemiesInRadius();
    }
    
    protected virtual void CheckForEnemiesInRadius()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        
        HashSet<EnemyView> currentEnemies = new HashSet<EnemyView>();
        
        foreach (var hit in hits)
        {
            if (hit != null && hit.TryGetComponent(out EnemyView enemy) && enemy != null)
            {
                currentEnemies.Add(enemy);
                
                if (!enemiesInZone.Contains(enemy))
                {
                    OnEnemyEnter(enemy);
                }
                else
                {
                    OnEnemyStay(enemy);
                }
            }
        }
        
        // Check for enemies that left (and remove nulls)
        HashSet<EnemyView> enemiesToRemove = new HashSet<EnemyView>();
        foreach (var enemy in enemiesInZone)
        {
            if (enemy == null)
            {
                enemiesToRemove.Add(enemy);
            }
            else if (!currentEnemies.Contains(enemy))
            {
                enemiesToRemove.Add(enemy);
                OnEnemyExit(enemy);
            }
        }
        
        // Remove enemies that left or are null
        foreach (var enemy in enemiesToRemove)
        {
            enemiesInZone.Remove(enemy);
        }
        
        enemiesInZone = currentEnemies;
    }
    
    /// <summary>
    /// Called when zone is first created
    /// </summary>
    protected abstract void OnZoneCreated();
    
    /// <summary>
    /// Called every frame while zone is active
    /// </summary>
    protected abstract void UpdateZone();
    
    /// <summary>
    /// Called when an enemy enters the zone
    /// </summary>
    protected abstract void OnEnemyEnter(EnemyView enemy);
    
    /// <summary>
    /// Called every frame while enemy is in zone
    /// </summary>
    protected abstract void OnEnemyStay(EnemyView enemy);
    
    /// <summary>
    /// Called when an enemy leaves the zone
    /// </summary>
    protected abstract void OnEnemyExit(EnemyView enemy);
    
    /// <summary>
    /// Called when zone expires naturally
    /// </summary>
    protected abstract void OnZoneExpired();
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
