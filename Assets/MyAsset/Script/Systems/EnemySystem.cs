using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CastleView castle;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float spawnInterval = 2f;

    private List<EnemyView> activeEnemies = new List<EnemyView>();
    private float castleXPosition;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<AttackCastleGA>(AttackCastlePerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AttackCastleGA>();
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
            enemy.transform.position += Vector3.left * moveSpeed * Time.deltaTime;

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
        
        Destroy(enemy.gameObject);
    }

    private IEnumerator AttackCastlePerformer(AttackCastleGA attackCastleGA)
    {
        // Create DealDamageGA and add as reaction
        List<CombatantView> targets = new List<CombatantView> { attackCastleGA.Castle };
        DealDamageGA dealDamageGA = new DealDamageGA(attackCastleGA.Damage, targets);
        ActionSystem.Instance.AddReaction(dealDamageGA);
        
        yield return null;
    }
}
