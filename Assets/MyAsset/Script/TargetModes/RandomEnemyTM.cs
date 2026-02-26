using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        if (EnemySystem.Instance.activeEnemies.Count == 0)
            return new List<CombatantView>();

        int randomIndex = Random.Range(0, EnemySystem.Instance.activeEnemies.Count);
        return new List<CombatantView> { EnemySystem.Instance.activeEnemies[randomIndex] };
    }
}
