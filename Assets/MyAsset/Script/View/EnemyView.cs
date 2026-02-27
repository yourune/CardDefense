using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private SpriteRenderer targetIndicator;
    
    public float Speed { get; private set; }
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
    
    public void ShowTargetIndicator(bool show)
    {
        if (targetIndicator != null)
        {
            targetIndicator.gameObject.SetActive(show);
        }
    }
}
