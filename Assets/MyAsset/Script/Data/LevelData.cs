using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/Level")]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    [Tooltip("Display name for this level/stage")]
    public string levelName = "Level 1";
    
    [Tooltip("Level description or objective")]
    [TextArea(2, 4)]
    public string levelDescription = "";
    
    [Header("Waves")]
    [Tooltip("All waves in this level, played in order")]
    public List<WaveData> waves = new List<WaveData>();
    
    [Header("Level Settings")]
    [Tooltip("Auto-start next wave after delay, or require manual button press")]
    public bool autoStartWaves = false;
    
    [Tooltip("Victory condition: clear all waves")]
    public bool victoryOnAllWavesCleared = true;
    
    [Header("Level Rewards")]
    [Tooltip("Bonus gold for completing the entire level")]
    public int levelCompletionGold = 100;
    
    [Tooltip("Bonus XP for completing the entire level")]
    public int levelCompletionXP = 50;
    
    [Header("References")]
    [Tooltip("Castle data for this level")]
    public CastleData castleData;
    
    /// <summary>
    /// Get total number of waves in this level
    /// </summary>
    public int TotalWaves => waves.Count;
    
    /// <summary>
    /// Check if level data is valid
    /// </summary>
    public bool IsValid()
    {
        return waves != null && waves.Count > 0 && castleData != null;
    }
}
