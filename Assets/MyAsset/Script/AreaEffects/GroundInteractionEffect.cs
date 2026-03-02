using UnityEngine;

/// <summary>
/// Effect that interacts with ground states (e.g., thunder on wet ground, ice on water)
/// </summary>
[System.Serializable]
public class GroundInteractionEffect : AreaEffect
{
    [SerializeField] private GroundStateType requiredGroundState;
    [SerializeField] private int baseDamage;
    [SerializeField] private int bonusDamageOnGroundState;
    [SerializeField] private float effectRadius = 3f;
    [SerializeField] private GroundStateType newGroundState = GroundStateType.None;
    [SerializeField] private float newGroundStateDuration = 5f;
    [SerializeField] private GameObject effectVFXPrefab;
    [SerializeField] private GameObject bonusVFXPrefab; // VFX when ground state is present
    [SerializeField] private LayerMask enemyLayer;
    
    public override GameAction[] GetGameActions(Vector3[] positions)
    {
        System.Collections.Generic.List<GameAction> actions = new System.Collections.Generic.List<GameAction>();
        
        foreach (Vector3 position in positions)
        {
            bool hasGroundState = ZoneManager.Instance.HasGroundState(position, requiredGroundState);
            int damage = hasGroundState ? baseDamage + bonusDamageOnGroundState : baseDamage;
            
            // Find enemies in radius
            Collider[] hits = Physics.OverlapSphere(position, effectRadius, enemyLayer);
            System.Collections.Generic.List<CombatantView> targets = new System.Collections.Generic.List<CombatantView>();
            
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out EnemyView enemy))
                {
                    targets.Add(enemy);
                }
            }
            
            // Deal damage
            if (targets.Count > 0)
            {
                actions.Add(new DealDamageGA(damage, targets));
            }
            
            // Spawn appropriate VFX
            GameObject vfxToSpawn = hasGroundState && bonusVFXPrefab != null ? bonusVFXPrefab : effectVFXPrefab;
            if (vfxToSpawn != null)
            {
                actions.Add(new SpawnVFXGA(vfxToSpawn, position));
            }
            
            // Change ground state
            if (newGroundState != GroundStateType.None)
            {
                actions.Add(new SetGroundStateGA(position, effectRadius, newGroundState, newGroundStateDuration));
            }
            else if (hasGroundState)
            {
                // Clear the ground state after interaction (e.g., ice freezes water)
                actions.Add(new ClearGroundStateGA(position, effectRadius));
            }
        }
        
        return actions.ToArray();
    }
}

/// <summary>
/// GameAction to set ground state in an area
/// </summary>
public class SetGroundStateGA : GameAction
{
    public Vector3 Position { get; private set; }
    public float Radius { get; private set; }
    public GroundStateType StateType { get; private set; }
    public float Duration { get; private set; }
    
    public SetGroundStateGA(Vector3 center, float radius, GroundStateType state, float duration)
    {
        this.Position = center;
        this.Radius = radius;
        this.StateType = state;
        this.Duration = duration;
    }
}

/// <summary>
/// GameAction to clear ground state in an area
/// </summary>
public class ClearGroundStateGA : GameAction
{
    public Vector3 Position { get; private set; }
    public float Radius { get; private set; }
    
    public ClearGroundStateGA(Vector3 center, float radius)
    {
        this.Position = center;
        this.Radius = radius;
    }
}
