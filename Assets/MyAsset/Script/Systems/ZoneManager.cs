using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all active effect zones and handles zone interactions
/// </summary>
public class ZoneManager : Singleton<ZoneManager>
{
    private List<EffectZone> activeZones = new List<EffectZone>();
    private Dictionary<Vector3, GroundState> groundStates = new Dictionary<Vector3, GroundState>();
    
    private const float GROUND_STATE_GRID_SIZE = 1f; // Ground states are snapped to grid
    
    public void RegisterZone(EffectZone zone)
    {
        if (!activeZones.Contains(zone))
        {
            activeZones.Add(zone);
        }
    }
    
    public void UnregisterZone(EffectZone zone)
    {
        activeZones.Remove(zone);
    }
    
    /// <summary>
    /// Get all zones at a position within a radius
    /// </summary>
    public List<EffectZone> GetZonesAtPosition(Vector3 position, float radius = 0.5f)
    {
        List<EffectZone> zones = new List<EffectZone>();
        
        foreach (var zone in activeZones)
        {
            float distance = Vector3.Distance(position, zone.transform.position);
            if (distance <= zone.Radius + radius)
            {
                zones.Add(zone);
            }
        }
        
        return zones;
    }
    
    /// <summary>
    /// Set ground state at a position (water, ice, etc.)
    /// </summary>
    public void SetGroundState(Vector3 position, GroundStateType stateType, float duration)
    {
        Vector3 gridPos = SnapToGrid(position);
        
        if (groundStates.TryGetValue(gridPos, out GroundState existing))
        {
            existing.stateType = stateType;
            existing.remainingTime = duration;
        }
        else
        {
            groundStates[gridPos] = new GroundState
            {
                stateType = stateType,
                remainingTime = duration,
                position = gridPos
            };
        }
    }
    
    /// <summary>
    /// Get ground state at position
    /// </summary>
    public GroundState GetGroundState(Vector3 position)
    {
        Vector3 gridPos = SnapToGrid(position);
        return groundStates.TryGetValue(gridPos, out GroundState state) ? state : null;
    }
    
    /// <summary>
    /// Check if ground has specific state type
    /// </summary>
    public bool HasGroundState(Vector3 position, GroundStateType stateType)
    {
        GroundState state = GetGroundState(position);
        return state != null && state.stateType == stateType;
    }
    
    /// <summary>
    /// Remove ground state at position
    /// </summary>
    public void ClearGroundState(Vector3 position)
    {
        Vector3 gridPos = SnapToGrid(position);
        groundStates.Remove(gridPos);
    }
    
    private void Update()
    {
        UpdateGroundStates();
        CleanupExpiredZones();
    }
    
    private void UpdateGroundStates()
    {
        List<Vector3> expiredPositions = new List<Vector3>();
        
        foreach (var kvp in groundStates)
        {
            kvp.Value.remainingTime -= Time.deltaTime;
            if (kvp.Value.remainingTime <= 0)
            {
                expiredPositions.Add(kvp.Key);
            }
        }
        
        foreach (var pos in expiredPositions)
        {
            groundStates.Remove(pos);
        }
    }
    
    private void CleanupExpiredZones()
    {
        activeZones.RemoveAll(zone => zone == null || zone.IsExpired);
    }
    
    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / GROUND_STATE_GRID_SIZE) * GROUND_STATE_GRID_SIZE,
            Mathf.Round(position.y / GROUND_STATE_GRID_SIZE) * GROUND_STATE_GRID_SIZE,
            Mathf.Round(position.z / GROUND_STATE_GRID_SIZE) * GROUND_STATE_GRID_SIZE
        );
    }
}

/// <summary>
/// Represents environmental ground state (wet, frozen, etc.)
/// </summary>
public class GroundState
{
    public GroundStateType stateType;
    public float remainingTime;
    public Vector3 position;
}

public enum GroundStateType
{
    None,
    Wet,        // From rain/water effects
    Frozen,     // From ice effects
    Burning,    // From fire effects
    Poisoned    // From poison effects
}
