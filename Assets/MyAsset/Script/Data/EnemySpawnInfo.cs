using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    [Tooltip("The enemy type to spawn")]
    public EnemyData enemyData;
    
    [Tooltip("Delay in seconds before spawning this enemy (relative to wave start)")]
    public float spawnDelay = 0f;
    
    [Tooltip("Optional position offset from spawn point")]
    public Vector3 spawnOffset = Vector3.zero;
}
