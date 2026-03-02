using UnityEngine;
using System.Collections.Generic;

public class MatchSetupSystem : MonoBehaviour
{
    [Header("Level Setup")]
    [SerializeField] private LevelData levelData;
    
    [Header("Legacy Setup (if not using LevelData)")]
    [SerializeField] private CastleData legacyCastleData;
    [SerializeField] private List<EnemyData> legacyEnemyDataList;
    
    private void Start()
    {
        if (levelData != null && levelData.IsValid())
        {
            // New wave-based system
            SetupLevelWithWaves();
        }
        else
        {
            // Legacy system for backward compatibility
            SetupLegacyMatch();
        }
    }
    
    private void SetupLevelWithWaves()
    {
        Debug.Log($"[MatchSetup] Starting level: {levelData.levelName}");
        
        // Setup castle
        CastleSystem.Instance.Setup(levelData.castleData);
        
        // Setup card system
        CardSystem.Instance.SetUp(levelData.castleData.Deck);
        
        // Draw initial cards
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
        
        // Initialize wave system
        WaveSystem.Instance.Initialize(levelData);
        
        // Auto-start first wave or wait for manual trigger
        if (levelData.autoStartWaves)
        {
            WaveSystem.Instance.StartNextWave();
        }
        else
        {
            Debug.Log("[MatchSetup] Wave system ready. Call WaveSystem.Instance.StartNextWave() to begin.");
        }
    }
    
    private void SetupLegacyMatch()
    {
        Debug.LogWarning("[MatchSetup] Using legacy setup. Consider creating a LevelData asset!");
        
        if (legacyCastleData != null)
        {
            CastleSystem.Instance.Setup(legacyCastleData);
            CardSystem.Instance.SetUp(legacyCastleData.Deck);
        }
        
        if (legacyEnemyDataList != null && legacyEnemyDataList.Count > 0)
        {
            EnemySystem.Instance.SpawnEnemy(legacyEnemyDataList);
        }
        
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
