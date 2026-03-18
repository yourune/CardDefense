using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private SpriteRenderer targetIndicator;
    
    public float Speed { get; set; }
    public EnemyData EnemyData { get; private set; }

    public void Setup(EnemyData enemyData)
    {
        EnemyData = enemyData;
        SetupBase(enemyData.Health, enemyData.Image);
        Speed = enemyData.Speed;
        
        if (targetIndicator != null)
        {
            targetIndicator.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Apply wave modifiers to this enemy
    /// </summary>
    public void ApplyModifiers(float healthMultiplier, float speedMultiplier)
    {
        if (healthMultiplier != 1f)
        {
            int modifiedMaxHealth = Mathf.RoundToInt(MaxHealth * healthMultiplier);
            int healthDifference = modifiedMaxHealth - MaxHealth;
            IncreaseMaxHealth(healthDifference);
        }
        
        if (speedMultiplier != 1f)
        {
            Speed *= speedMultiplier;
        }
    }
    
    public void ShowTargetIndicator(bool show)
    {
        if (targetIndicator != null)
        {
            targetIndicator.gameObject.SetActive(show);
        }
    }
}
