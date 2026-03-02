using UnityEngine;

/// <summary>
/// GameAction that spawns a VFX at a position
/// </summary>
public class SpawnVFXGA : GameAction
{
    public GameObject VFXPrefab { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    
    public SpawnVFXGA(GameObject vfxPrefab, Vector3 position, Quaternion rotation = default)
    {
        this.VFXPrefab = vfxPrefab;
        this.Position = position;
        this.Rotation = rotation == default ? Quaternion.identity : rotation;
    }
}
