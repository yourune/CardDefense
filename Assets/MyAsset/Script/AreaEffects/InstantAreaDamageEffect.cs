using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantAreaDamageEffect : AreaEffect
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float effectRadius = 3f;
    [SerializeField] private GameObject explosionVFXPrefab;
    [SerializeField] private LayerMask enemyLayer;
    
    public override GameAction[] GetGameActions(Vector3[] positions)
    {
        List<GameAction> actions = new List<GameAction>();
        
        foreach (Vector3 position in positions)
        {
            // Find all enemies in radius
            Collider[] hits = Physics.OverlapSphere(position, effectRadius, enemyLayer);
            List<CombatantView> targets = new List<CombatantView>();
            
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out EnemyView enemy))
                {
                    targets.Add(enemy);
                }
            }
            
            // Create damage action
            if (targets.Count > 0)
            {
                actions.Add(new DealDamageGA(damageAmount, targets));
            }
            
            // Spawn VFX
            if (explosionVFXPrefab != null)
            {
                actions.Add(new SpawnVFXGA(explosionVFXPrefab, position));
            }
        }
        
        return actions.ToArray();
    }
}
