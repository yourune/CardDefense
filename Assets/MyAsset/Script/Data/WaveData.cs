using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/Wave")]
public class WaveData : ScriptableObject
{
    [Header("Wave Enemies")]
    [Tooltip("List of enemies to spawn in this wave with individual timing")]
    public List<EnemySpawnInfo> enemiesToSpawn = new List<EnemySpawnInfo>();
    
    [Header("Wave Modifiers")]
    [Tooltip("Health multiplier for all enemies in this wave")]
    [Range(0.5f, 5f)]
    public float healthMultiplier = 1f;
    
    [Tooltip("Speed multiplier for all enemies in this wave")]
    [Range(0.5f, 3f)]
    public float speedMultiplier = 1f;
    
    [Tooltip("Damage multiplier when enemies reach the castle")]
    [Range(0.5f, 5f)]
    public float damageMultiplier = 1f;
    
    [Header("Wave Settings")]
    [Tooltip("Delay before next wave can start (after this wave is cleared)")]
    public float nextWaveDelay = 3f;
    
    [Tooltip("Is this a boss wave? (visual/audio effects)")]
    public bool isBossWave = false;
    
    [Header("Wave Rewards")]
    [Tooltip("Gold awarded when wave is completed")]
    public int waveCompletionGold = 0;
}
