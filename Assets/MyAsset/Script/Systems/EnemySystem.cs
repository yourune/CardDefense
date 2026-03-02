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
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AttackCastleGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
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
        
        activeEnemies.Remove(killEnemyGA.EnemyView);
        
        if (killEnemyGA.EnemyView != null && killEnemyGA.EnemyView.gameObject != null)
        {
            killEnemyGA.EnemyView.gameObject.SetActive(false);
            Destroy(killEnemyGA.EnemyView.gameObject);
        }
        
        yield return null;
    }
}
