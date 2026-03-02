using UnityEngine;

/// <summary>
/// Rain effect that creates wet ground state for interactions
/// </summary>
public class RainZone : EffectZone
{
    [SerializeField] private GameObject rainVFX;
    [SerializeField] private float groundStateRadius = 2f;
    
    protected override void OnZoneCreated()
    {
        if (rainVFX != null)
        {
            Instantiate(rainVFX, transform.position, Quaternion.identity, transform);
        }
        
        // Create initial wet ground state
        CreateWetGround();
    }
    
    protected override void UpdateZone()
    {
        // Continuously refresh wet ground state
        CreateWetGround();
    }
    
    private void CreateWetGround()
    {
        // Create wet spots in a grid pattern around the zone
        for (float x = -radius; x <= radius; x += 1f)
        {
            for (float z = -radius; z <= radius; z += 1f)
            {
                Vector3 groundPos = transform.position + new Vector3(x, 0, z);
                if (Vector3.Distance(groundPos, transform.position) <= radius)
                {
                    ZoneManager.Instance.SetGroundState(groundPos, GroundStateType.Wet, duration);
                }
            }
        }
    }
    
    protected override void OnEnemyEnter(EnemyView enemy)
    {
        // Enemies become wet (could add wet debuff)
    }
    
    protected override void OnEnemyStay(EnemyView enemy)
    {
        // Keep enemies wet
    }
    
    protected override void OnEnemyExit(EnemyView enemy)
    {
        // Enemies stay wet for a while after leaving
    }
    
    protected override void OnZoneExpired()
    {
        // Wet ground persists after rain stops
        Debug.Log("Rain stopped, but ground remains wet");
    }
}
