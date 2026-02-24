using UnityEngine;

public class EnemyView : CombatantView
{
    public void Setup(EnemyData enemyData)
    {
        SetupBase(enemyData.Health, enemyData.Image);
    }
    
}
