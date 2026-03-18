using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CastleView castle;
    [SerializeField] private float spawnInterval = 2f;

    public List<EnemyView> activeEnemies = new List<EnemyView>();
    private float castleXPosition;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<AttackCastleGA>(AttackCastlePerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
        ActionSystem.AttachPerformer<KillMultipleEnemiesGA>(KillMultipleEnemiesPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AttackCastleGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
        ActionSystem.DetachPerformer<KillMultipleEnemiesGA>();
    }

    private void Start()
    {
        if (castle != null)
        {
            castleXPosition = castle.transform.position.x;
        }
    }

    private void Update()
    {
        MoveEnemies();
        CheckAndHandleDeadEnemies();
    }
    
    /// <summary>
    /// Check for dead enemies and batch process them through ActionSystem.
    /// This centralizes death detection - Zone/DamageSystem only need to call TakeDamage().
    /// </summary>
    private void CheckAndHandleDeadEnemies()
    {
        List<EnemyView> deadEnemies = new List<EnemyView>();
        
        // Detect all dead enemies
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null && enemy.currentHealth <= 0)
            {
                enemy.Speed = 0f; // Stop immediately
                deadEnemies.Add(enemy);
            }
        }
        
        // Remove dead enemies from active list immediately (prevent duplicate detection)
        if (deadEnemies.Count > 0)
        {
            foreach (var enemy in deadEnemies)
            {
                activeEnemies.Remove(enemy);
            }
            
            // Batch process through ActionSystem
            if (deadEnemies.Count == 1)
            {
                KillEnemyGA killEnemyGA = new KillEnemyGA(deadEnemies[0]);
                ActionSystem.Instance.Perform(killEnemyGA);
            }
            else
            {
                KillMultipleEnemiesGA killMultipleGA = new KillMultipleEnemiesGA(deadEnemies);
                ActionSystem.Instance.Perform(killMultipleGA);
            }
        }
    }

    public void SpawnEnemy(List<EnemyData> enemyDataList)
    {
        StartCoroutine(SpawnEnemiesCoroutine(enemyDataList));
    }
    
    /// <summary>
    /// Spawn a single enemy with wave modifiers and position offset
    /// </summary>
    public void SpawnEnemyWithModifiers(EnemyData enemyData, Vector3 positionOffset, float healthMultiplier, float speedMultiplier)
    {
        if (EnemyViewCreator.Instance == null || spawnPoint == null)
        {
            Debug.LogError("EnemyViewCreator or SpawnPoint is not assigned!");
            return;
        }
        
        Vector3 spawnPosition = spawnPoint.position + positionOffset;
        EnemyView enemy = EnemyViewCreator.Instance.CreateEnemyView(enemyData, spawnPosition, Quaternion.identity);
        
        if (enemy != null)
        {
            // Apply wave modifiers
            enemy.ApplyModifiers(healthMultiplier, speedMultiplier);
            activeEnemies.Add(enemy);
        }
    }
    
    /// <summary>
    /// Get the number of active enemies
    /// </summary>
    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
    }

    private IEnumerator SpawnEnemiesCoroutine(List<EnemyData> enemyDataList)
    {
        foreach (var enemyData in enemyDataList)
        {
            SpawnSingleEnemy(enemyData);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnSingleEnemy(EnemyData enemyData)
    {
        if (EnemyViewCreator.Instance == null || spawnPoint == null)
        {
            Debug.LogError("EnemyViewCreator or SpawnPoint is not assigned!");
            return;
        }

        EnemyView enemy = EnemyViewCreator.Instance.CreateEnemyView(enemyData, spawnPoint.position, Quaternion.identity);
        if (enemy != null)
        {
            activeEnemies.Add(enemy);
        }
    }

    private void MoveEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            EnemyView enemy = activeEnemies[i];
            
            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            // Move enemy left
            enemy.transform.position += Vector3.left * enemy.Speed * Time.deltaTime;

            // Check if enemy reached the castle
            if (enemy.transform.position.x <= castleXPosition)
            {
                OnEnemyReachedCastle(enemy);
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private void OnEnemyReachedCastle(EnemyView enemy)
    {
        if (castle != null)
        {
            AttackCastleGA attackCastleGA = new AttackCastleGA(castle, 1);
            ActionSystem.Instance.Perform(attackCastleGA);
            Debug.Log("Enemy reached castle!");
        }
        
        enemy.gameObject.SetActive(false);
        Destroy(enemy.gameObject);
    }
    
    /// <summary>
    /// Remove enemy directly without using ActionSystem (for zones)
    /// </summary>
    public void RemoveEnemyDirect(EnemyView enemy)
    {
        if (enemy == null) return;
        
        activeEnemies.Remove(enemy);
        
        if (enemy.gameObject != null)
        {
            enemy.gameObject.SetActive(false);
            Destroy(enemy.gameObject);
        }
    }

    private IEnumerator AttackCastlePerformer(AttackCastleGA attackCastleGA)
    {
        // Create DealDamageGA and add as reaction
        List<CombatantView> targets = new List<CombatantView> { attackCastleGA.Castle };
        DealDamageGA dealDamageGA = new DealDamageGA(attackCastleGA.Damage, targets);
        ActionSystem.Instance.AddReaction(dealDamageGA);
        
        yield return null;
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        if (killEnemyGA.EnemyView == null)
        {
            yield break;
        }
        
        // Store enemy data before destroying
        EnemyData enemyData = killEnemyGA.EnemyView.EnemyData;
        Vector3 enemyPosition = killEnemyGA.EnemyView.transform.position;
        
        // Remove from active list
        activeEnemies.Remove(killEnemyGA.EnemyView);
        
        // Destroy the enemy
        if (killEnemyGA.EnemyView != null && killEnemyGA.EnemyView.gameObject != null)
        {
            killEnemyGA.EnemyView.gameObject.SetActive(false);
            Destroy(killEnemyGA.EnemyView.gameObject);
        }
        
        // Wait a frame to ensure enemy is destroyed
        yield return null;
        
        // Now drop rewards after enemy is gone
        if (enemyData != null)
        {
            DropRewardGA dropRewardGA = new DropRewardGA(
                enemyPosition,
                enemyData.GoldDrop,
                enemyData.XpDrop
            );
            ActionSystem.Instance.AddReaction(dropRewardGA);
        }
    }
    
    /// <summary>
    /// Kill multiple enemies simultaneously (all disappear at once, then rewards spawn together)
    /// </summary>
    private IEnumerator KillMultipleEnemiesPerformer(KillMultipleEnemiesGA killMultipleEnemiesGA)
    {
        if (killMultipleEnemiesGA.Enemies == null || killMultipleEnemiesGA.Enemies.Count == 0)
        {
            yield break;
        }
        
        // Store all enemy data before destroying
        List<(Vector3 position, int gold, int xp)> rewardData = new List<(Vector3, int, int)>();
        
        foreach (var enemy in killMultipleEnemiesGA.Enemies)
        {
            if (enemy != null)
            {
                // Collect reward data
                if (enemy.EnemyData != null)
                {
                    rewardData.Add((
                        enemy.transform.position, 
                        enemy.EnemyData.GoldDrop, 
                        enemy.EnemyData.XpDrop
                    ));
                }
                
                // Remove from active list
                activeEnemies.Remove(enemy);
                
                // Destroy the enemy immediately
                if (enemy.gameObject != null)
                {
                    enemy.gameObject.SetActive(false);
                    Destroy(enemy.gameObject);
                }
            }
        }
        
        // Wait a frame to ensure all enemies are destroyed
        yield return null;
        
        // Drop all rewards at once using batch processing
        if (rewardData.Count > 0)
        {
            DropMultipleRewardsGA dropMultipleRewardsGA = new DropMultipleRewardsGA(rewardData);
            ActionSystem.Instance.AddReaction(dropMultipleRewardsGA);
        }
    }
}
