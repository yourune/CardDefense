using UnityEngine;

/// <summary>
/// GameAction that spawns an effect zone at a position
/// </summary>
public class SpawnEffectZoneGA : GameAction
{
    public GameObject ZonePrefab { get; private set; }
    public Vector3 Position { get; private set; }
    public float Duration { get; private set; }
    public float Radius { get; private set; }
    
    public SpawnEffectZoneGA(GameObject zonePrefab, Vector3 position, float duration, float radius)
    {
        this.ZonePrefab = zonePrefab;
        this.Position = position;
        this.Duration = duration;
        this.Radius = radius;
    }
}
