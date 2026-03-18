using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all zone effects (poison, tornado, rain, etc.)
/// All zones follow the same mechanism:
/// 1. Card use → Zone spawned
/// 2. VFX effect plays + Zone image displayed on ground
/// 3. Zone deals damage per tick (can be 0)
/// 4. Special effects implemented by child classes (slow, pull, poison stack, etc.)
/// 
/// Setup Requirements:
/// - BoxCollider (isTrigger = true): All zones are rectangular
/// - Rigidbody (isKinematic = true): Required for triggers
/// - Box Height (Y): Should be large enough to collide with all enemy sizes
/// - Enemy must have: Collider + Rigidbody
/// </summary>
public abstract class EffectZone : MonoBehaviour
{
    [Header("Zone Base Settings")]
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float width = 5f;   // Zone width (X axis)
    [SerializeField] protected float height = 5f;  // Zone height (Z axis)
    
    [Header("Visuals")]
    [SerializeField] protected GameObject zoneVFXPrefab;      // VFX effect (particles, etc.)
    [SerializeField] protected GameObject zoneGroundImage;    // Ground image that remains visible
    
    [Header("Damage Settings")]
    [SerializeField] protected int damagePerTick = 0;         // Damage per tick (0 = no damage)
    [SerializeField] protected float damageTickInterval = 1f; // Interval between damage ticks
    
    protected float remainingTime;
    protected float nextDamageTick;
    protected HashSet<EnemyView> enemiesInZone = new HashSet<EnemyView>();
    
    private GameObject spawnedVFX;
    private GameObject spawnedGroundImage;
    
    public float Width => width;
    public float Height => height;
    public bool IsExpired => remainingTime <= 0;
    
    protected virtual void Start()
    {
        remainingTime = duration;
        nextDamageTick = Time.time + damageTickInterval;
        
        // Spawn VFX and ground image automatically
        SpawnVisuals();
        
        // Call child class initialization
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
        
        // Handle damage automatically
        if (damagePerTick > 0 && Time.time >= nextDamageTick)
        {
            nextDamageTick = Time.time + damageTickInterval;
            DealDamageToEnemiesInZone(damagePerTick);
        }
        
        // Child class custom update logic
        UpdateZone();
    }
    
    // ========== Unity Trigger Events ==========
    // Zone must have: Collider (isTrigger=true) + Rigidbody (isKinematic=true)
    // Enemy must have: Collider + Rigidbody
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyView enemy))
        {
            enemiesInZone.Add(enemy);
            OnEnemyEnter(enemy);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out EnemyView enemy))
        {
            OnEnemyStay(enemy);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyView enemy))
        {
            enemiesInZone.Remove(enemy);
            OnEnemyExit(enemy);
        }
    }
    
    /// <summary>
    /// Spawn VFX and ground image
    /// </summary>
    private void SpawnVisuals()
    {
        if (zoneVFXPrefab != null)
        {
            spawnedVFX = Instantiate(zoneVFXPrefab, transform.position, Quaternion.identity, transform);
        }
        
        if (zoneGroundImage != null)
        {
            spawnedGroundImage = Instantiate(zoneGroundImage, transform.position, Quaternion.identity, transform);
        }
    }
    
    /// <summary>
    /// Deal damage to all enemies in zone.
    /// Death detection is handled automatically by EnemySystem.
    /// </summary>
    protected void DealDamageToEnemiesInZone(int damage)
    {
        foreach (var enemy in enemiesInZone)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    
    // ========== Virtual Methods (Optional Override) ==========
    
    /// <summary>
    /// Called once when zone is created. Override for initialization.
    /// </summary>
    protected virtual void OnZoneCreated() { }
    
    /// <summary>
    /// Called every frame. Override for special zone logic (pull enemies, etc.)
    /// </summary>
    protected virtual void UpdateZone() { }
    
    /// <summary>
    /// Called when an enemy enters the zone. Override for special effects.
    /// </summary>
    protected virtual void OnEnemyEnter(EnemyView enemy) { }
    
    /// <summary>
    /// Called every frame while enemy is in zone. Override for continuous effects.
    /// </summary>
    protected virtual void OnEnemyStay(EnemyView enemy) { }
    
    /// <summary>
    /// Called when an enemy leaves the zone. Override for cleanup.
    /// </summary>
    protected virtual void OnEnemyExit(EnemyView enemy) { }
    
    /// <summary>
    /// Called when zone expires. Override for cleanup.
    /// </summary>
    protected virtual void OnZoneExpired() { }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 10f, height));
    }
}
