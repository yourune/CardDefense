using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System that performs area effect game actions (spawning zones, VFX, traveling waves)
/// </summary>
public class AreaEffectSystem : Singleton<AreaEffectSystem>
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpawnEffectZoneGA>(SpawnEffectZonePerformer);
        ActionSystem.AttachPerformer<SpawnVFXGA>(SpawnVFXPerformer);
        ActionSystem.AttachPerformer<TravelingWaveGA>(TravelingWavePerformer);
        ActionSystem.AttachPerformer<SetGroundStateGA>(SetGroundStatePerformer);
        ActionSystem.AttachPerformer<ClearGroundStateGA>(ClearGroundStatePerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpawnEffectZoneGA>();
        ActionSystem.DetachPerformer<SpawnVFXGA>();
        ActionSystem.DetachPerformer<TravelingWaveGA>();
        ActionSystem.DetachPerformer<SetGroundStateGA>();
        ActionSystem.DetachPerformer<ClearGroundStateGA>();
    }
    
    private IEnumerator SpawnEffectZonePerformer(SpawnEffectZoneGA action)
    {
        if (action.ZonePrefab == null)
        {
            Debug.LogError("SpawnEffectZoneGA: ZonePrefab is null!");
            yield break;
        }
        
        GameObject zoneObj = Instantiate(action.ZonePrefab, action.Position, Quaternion.identity);
        
        if (zoneObj.TryGetComponent(out EffectZone zone))
        {
            ZoneManager.Instance.RegisterZone(zone);
        }
        else
        {
            Debug.LogError($"SpawnEffectZoneGA: {action.ZonePrefab.name} doesn't have EffectZone component!");
        }
        
        yield return null;
    }
    
    private IEnumerator SpawnVFXPerformer(SpawnVFXGA action)
    {
        if (action.VFXPrefab != null)
        {
            Instantiate(action.VFXPrefab, action.Position, action.Rotation);
        }
        
        yield return null;
    }
    
    private IEnumerator TravelingWavePerformer(TravelingWaveGA action)
    {
        float distanceTraveled = 0f;
        Vector3 currentPosition = action.StartPosition;
        HashSet<EnemyView> hitEnemies = new HashSet<EnemyView>();
        
        GameObject waveVFX = null;
        if (action.VFXPrefab != null)
        {
            waveVFX = Instantiate(action.VFXPrefab, currentPosition, Quaternion.LookRotation(action.Direction));
        }
        
        while (distanceTraveled < action.MaxDistance)
        {
            float moveDistance = action.Speed * Time.deltaTime;
            currentPosition += action.Direction * moveDistance;
            distanceTraveled += moveDistance;
            
            if (waveVFX != null)
            {
                waveVFX.transform.position = currentPosition;
            }
            
            // Check for enemies in wave path
            Collider[] hits = Physics.OverlapBox(
                currentPosition,
                new Vector3(action.Width / 2f, 2f, 0.5f),
                Quaternion.LookRotation(action.Direction),
                action.EnemyLayer
            );
            
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out EnemyView enemy) && !hitEnemies.Contains(enemy))
                {
                    hitEnemies.Add(enemy);
                    ActionSystem.Instance.AddReaction(new DealDamageGA(action.DamageAmount, new List<CombatantView> { enemy }));
                }
            }
            
            yield return null;
        }
        
        if (waveVFX != null)
        {
            Destroy(waveVFX, 2f);
        }
    }
    
    private IEnumerator SetGroundStatePerformer(SetGroundStateGA action)
    {
        // Set ground state in a grid pattern
        for (float x = -action.Radius; x <= action.Radius; x += 1f)
        {
            for (float z = -action.Radius; z <= action.Radius; z += 1f)
            {
                Vector3 groundPos = action.Position + new Vector3(x, 0, z);
                if (Vector3.Distance(groundPos, action.Position) <= action.Radius)
                {
                    ZoneManager.Instance.SetGroundState(groundPos, action.StateType, action.Duration);
                }
            }
        }
        
        yield return null;
    }
    
    private IEnumerator ClearGroundStatePerformer(ClearGroundStateGA action)
    {
        for (float x = -action.Radius; x <= action.Radius; x += 1f)
        {
            for (float z = -action.Radius; z <= action.Radius; z += 1f)
            {
                Vector3 groundPos = action.Position + new Vector3(x, 0, z);
                if (Vector3.Distance(groundPos, action.Position) <= action.Radius)
                {
                    ZoneManager.Instance.ClearGroundState(groundPos);
                }
            }
        }
        
        yield return null;
    }
}
