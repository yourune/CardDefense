using UnityEngine;

/// <summary>
/// Square-shaped rain zone
/// </summary>
public class SquareRainZone : EffectZone
{
    [SerializeField] private GameObject rainVFX;
    [SerializeField] private float width = 5f;  // Width of square
    [SerializeField] private float height = 5f; // Height of square
    
    protected override void Start()
    {
        remainingTime = duration;
        OnZoneCreated();
    }
    
    protected override void OnZoneCreated()
    {
        if (rainVFX != null)
        {
            // Spawn rain VFX at center
            GameObject vfx = Instantiate(rainVFX, transform.position, Quaternion.identity, transform);
            
            // Scale VFX to match square dimensions
            vfx.transform.localScale = new Vector3(width / 5f, 1f, height / 5f);
        }
        
        CreateWetGround();
    }
    
    protected override void UpdateZone()
    {
        // Continuously refresh wet ground state
        CreateWetGround();
    }
    
    private void CreateWetGround()
    {
        // Create wet spots in a square grid pattern
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;
        
        for (float x = -halfWidth; x <= halfWidth; x += 1f)
        {
            for (float z = -halfHeight; z <= halfHeight; z += 1f)
            {
                Vector3 groundPos = transform.position + new Vector3(x, 0, z);
                ZoneManager.Instance.SetGroundState(groundPos, GroundStateType.Wet, duration);
            }
        }
    }
    
    protected override void CheckForEnemiesInRadius()
    {
        // Check for enemies in square bounds instead of circle
        Collider[] hits = Physics.OverlapBox(
            transform.position,
            new Vector3(width / 2f, 2f, height / 2f),
            Quaternion.identity,
            enemyLayer
        );
        
        System.Collections.Generic.HashSet<EnemyView> currentEnemies = new System.Collections.Generic.HashSet<EnemyView>();
        
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyView enemy))
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
        
        // Check for enemies that left
        foreach (var enemy in enemiesInZone)
        {
            if (!currentEnemies.Contains(enemy))
            {
                OnEnemyExit(enemy);
            }
        }
        
        enemiesInZone = currentEnemies;
    }
    
    protected override void OnEnemyEnter(EnemyView enemy)
    {
        // Enemy becomes wet when entering rain
        Debug.Log($"{enemy.name} is getting wet from rain");
    }
    
    protected override void OnEnemyStay(EnemyView enemy)
    {
        // Enemies stay wet while in rain
    }
    
    protected override void OnEnemyExit(EnemyView enemy)
    {
        Debug.Log($"{enemy.name} left the rain");
    }
    
    protected override void OnZoneExpired()
    {
        Debug.Log("Rain stopped, ground stays wet for a while");
    }
    
    protected override void OnDrawGizmosSelected()
    {
        // Draw square bounds in editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 2f, height));
    }
}
