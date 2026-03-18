using UnityEngine;
using System.Collections;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFXPrefab;
    [SerializeField] private int vfxPoolSize = 15;
    
    private SimpleObjectPool<PooledVFX> vfxPool;
    private Transform poolParent;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
        vfxPool?.ReturnAll();
    }
    
    private void Start()
    {
        // Initialize VFX pool
        if (damageVFXPrefab != null)
        {
            poolParent = new GameObject("DamageVFXPool").transform;
            poolParent.SetParent(transform);
            vfxPool = new SimpleObjectPool<PooledVFX>(damageVFXPrefab, poolParent, vfxPoolSize);
        }
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        foreach (var target in dealDamageGA.Targets)
        {
            if (target != null)
            {
                target.TakeDamage(dealDamageGA.DamageAmount);
                
                // Spawn VFX from pool
                if (vfxPool != null)
                {
                    PooledVFX vfx = vfxPool.Get();
                    vfx.transform.position = target.transform.position;
                    vfx.transform.rotation = Quaternion.identity;
                    vfx.SetReturnCallback((v) => vfxPool.Return(v));
                    vfx.ResetVFX();
                    
                    yield return new WaitForSeconds(0.15f);
                    
                    // Check if target died (Castle only - enemies handled by EnemySystem)
                    if (target != null && target.currentHealth <= 0 && target is CastleView)
                    {
                        // Do some game over logic here
                        // Open Game Over Scene
                    }
                }
            }
        }
    }   
}
