using System;
using System.Collections;
using UnityEngine;

public class WaveSystem : Singleton<WaveSystem>
{
    public enum WaveState
    {
        NotStarted,
        WaitingToStart,
        Spawning,
        Fighting,
        WaveComplete,
        AllWavesComplete
    }
    
    // Events
    public event Action<int> OnWaveStart; // Passes wave number (1-indexed)
    public event Action<int, WaveData> OnWaveComplete; // Passes wave number and wave data
    public event Action OnAllWavesComplete;
    public event Action<int, int> OnWaveCountChanged; // Passes current wave, total waves
    
    // State
    private LevelData currentLevel;
    private int currentWaveIndex = -1;
    private WaveState currentState = WaveState.NotStarted;
    private float waveStartTime;
    private Coroutine nextWaveCoroutine;
    
    // Properties
    public WaveState CurrentState => currentState;
    public int CurrentWaveNumber => currentWaveIndex + 1; // 1-indexed for display
    public int TotalWaves => currentLevel?.TotalWaves ?? 0;
    public WaveData CurrentWave => (currentLevel != null && currentWaveIndex >= 0 && currentWaveIndex < currentLevel.waves.Count) 
        ? currentLevel.waves[currentWaveIndex] 
        : null;
    public float WaveElapsedTime => Time.time - waveStartTime;
    public bool CanStartNextWave => currentState == WaveState.WaitingToStart;
    
    private void Update()
    {
        // Check for wave completion only when fighting
        if (currentState == WaveState.Fighting)
        {
            CheckWaveCompletion();
        }
    }
    
    /// <summary>
    /// Initialize the wave system with level data
    /// </summary>
    public void Initialize(LevelData levelData)
    {
        if (levelData == null || !levelData.IsValid())
        {
            Debug.LogError("[WaveSystem] Invalid LevelData!");
            return;
        }
        
        currentLevel = levelData;
        currentWaveIndex = -1;
        currentState = WaveState.WaitingToStart;
        
        Debug.Log($"[WaveSystem] Initialized with {levelData.levelName} - {levelData.TotalWaves} waves");
        
        OnWaveCountChanged?.Invoke(0, levelData.TotalWaves);
    }
    
    /// <summary>
    /// Start the next wave
    /// </summary>
    public void StartNextWave()
    {
        if (currentLevel == null)
        {
            Debug.LogError("[WaveSystem] No level initialized!");
            return;
        }
        
        if (currentState != WaveState.WaitingToStart)
        {
            Debug.LogWarning($"[WaveSystem] Cannot start wave in state: {currentState}");
            return;
        }
        
        currentWaveIndex++;
        
        if (currentWaveIndex >= currentLevel.TotalWaves)
        {
            Debug.LogWarning("[WaveSystem] No more waves to start!");
            return;
        }
        
        StartCoroutine(SpawnWaveCoroutine());
    }
    
    /// <summary>
    /// Spawn all enemies for the current wave
    /// </summary>
    private IEnumerator SpawnWaveCoroutine()
    {
        currentState = WaveState.Spawning;
        waveStartTime = Time.time;
        
        WaveData wave = CurrentWave;
        
        Debug.Log($"[WaveSystem] Starting Wave {CurrentWaveNumber}/{TotalWaves}" + 
                  (wave.isBossWave ? " [BOSS WAVE]" : ""));
        
        OnWaveStart?.Invoke(CurrentWaveNumber);
        OnWaveCountChanged?.Invoke(CurrentWaveNumber, TotalWaves);
        
        // Sort enemies by spawn delay to spawn in order
        var sortedEnemies = new System.Collections.Generic.List<EnemySpawnInfo>(wave.enemiesToSpawn);
        sortedEnemies.Sort((a, b) => a.spawnDelay.CompareTo(b.spawnDelay));
        
        float previousDelay = 0f;
        
        foreach (var spawnInfo in sortedEnemies)
        {
            // Wait for the delay difference
            float waitTime = spawnInfo.spawnDelay - previousDelay;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }
            
            // Spawn the enemy with modifiers
            EnemySystem.Instance.SpawnEnemyWithModifiers(
                spawnInfo.enemyData,
                spawnInfo.spawnOffset,
                wave.healthMultiplier,
                wave.speedMultiplier
            );
            
            previousDelay = spawnInfo.spawnDelay;
        }
        
        Debug.Log($"[WaveSystem] Wave {CurrentWaveNumber} spawning complete");
        currentState = WaveState.Fighting;
    }
    
    /// <summary>
    /// Check if all enemies in the current wave are defeated
    /// </summary>
    private void CheckWaveCompletion()
    {
        if (EnemySystem.Instance.GetActiveEnemyCount() == 0)
        {
            CompleteCurrentWave();
        }
    }
    
    /// <summary>
    /// Complete the current wave and handle rewards
    /// </summary>
    private void CompleteCurrentWave()
    {
        if (currentState != WaveState.Fighting) return;
        
        currentState = WaveState.WaveComplete;
        
        WaveData wave = CurrentWave;
        float waveTime = WaveElapsedTime;
        
        Debug.Log($"[WaveSystem] Wave {CurrentWaveNumber} completed in {waveTime:F1}s");
        
        // Calculate and grant rewards
        GrantWaveRewards(wave, waveTime);
        
        OnWaveComplete?.Invoke(CurrentWaveNumber, wave);
        
        // Check if all waves are complete
        if (currentWaveIndex >= currentLevel.TotalWaves - 1)
        {
            CompleteAllWaves();
        }
        else
        {
            // Prepare for next wave
            if (currentLevel.autoStartWaves)
            {
                nextWaveCoroutine = StartCoroutine(AutoStartNextWaveCoroutine(wave.nextWaveDelay));
            }
            else
            {
                currentState = WaveState.WaitingToStart;
                Debug.Log($"[WaveSystem] Ready for Wave {CurrentWaveNumber + 1}. Call StartNextWave() when ready.");
            }
        }
    }
    
    /// <summary>
    /// Auto-start next wave after delay
    /// </summary>
    private IEnumerator AutoStartNextWaveCoroutine(float delay)
    {
        currentState = WaveState.WaitingToStart;
        
        Debug.Log($"[WaveSystem] Next wave starts in {delay}s");
        yield return new WaitForSeconds(delay);
        
        StartNextWave();
    }
    
    /// <summary>
    /// Grant rewards for wave completion
    /// </summary>
    private void GrantWaveRewards(WaveData wave, float completionTime)
    {
        int goldReward = wave.waveCompletionGold;
        
        // Grant gold reward through ActionSystem
        if (goldReward > 0)
        {
            GainGoldGA gainGoldGA = new GainGoldGA(goldReward);
            ActionSystem.Instance.Perform(gainGoldGA);
            Debug.Log($"[WaveSystem] Wave Reward: {goldReward} gold");
        }
    }
    
    /// <summary>
    /// Complete all waves in the level
    /// </summary>
    private void CompleteAllWaves()
    {
        currentState = WaveState.AllWavesComplete;
        
        Debug.Log($"[WaveSystem] All waves completed! Level {currentLevel.levelName} finished!");
        
        // Grant level completion rewards
        if (currentLevel.levelCompletionGold > 0)
        {
            GainGoldGA gainGoldGA = new GainGoldGA(currentLevel.levelCompletionGold);
            ActionSystem.Instance.Perform(gainGoldGA);
        }
        
        if (currentLevel.levelCompletionXP > 0)
        {
            GainXpGA gainXpGA = new GainXpGA(currentLevel.levelCompletionXP);
            ActionSystem.Instance.Perform(gainXpGA);
        }
        
        if (currentLevel.levelCompletionGold > 0 || currentLevel.levelCompletionXP > 0)
        {
            Debug.Log($"[WaveSystem] Level Rewards: {currentLevel.levelCompletionGold} gold, {currentLevel.levelCompletionXP} XP");
        }
        
        OnAllWavesComplete?.Invoke();
    }
    
    /// <summary>
    /// Skip the countdown and start next wave immediately (for manual or skip button)
    /// </summary>
    public void SkipToNextWave()
    {
        if (nextWaveCoroutine != null)
        {
            StopCoroutine(nextWaveCoroutine);
            nextWaveCoroutine = null;
        }
        
        if (currentState == WaveState.WaitingToStart)
        {
            StartNextWave();
        }
    }
    
    /// <summary>
    /// Get active enemy count
    /// </summary>
    public int GetRemainingEnemies()
    {
        return EnemySystem.Instance.GetActiveEnemyCount();
    }
    
    /// <summary>
    /// Reset the wave system
    /// </summary>
    public void Reset()
    {
        if (nextWaveCoroutine != null)
        {
            StopCoroutine(nextWaveCoroutine);
            nextWaveCoroutine = null;
        }
        
        currentLevel = null;
        currentWaveIndex = -1;
        currentState = WaveState.NotStarted;
        waveStartTime = 0;
    }
}
