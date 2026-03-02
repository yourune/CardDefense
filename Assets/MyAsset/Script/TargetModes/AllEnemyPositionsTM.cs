using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllEnemyPositionsTM : PositionTargetMode
{
    /// <summary>
    /// Returns positions of all active enemies
    /// </summary>
    public override Vector3[] GetPositions()
    {
        if (EnemySystem.Instance == null || EnemySystem.Instance.activeEnemies.Count == 0)
        {
            return new Vector3[0];
        }
        
        List<Vector3> positions = new List<Vector3>();
        foreach (var enemy in EnemySystem.Instance.activeEnemies)
        {
            if (enemy != null)
            {
                positions.Add(enemy.transform.position);
            }
        }
        
        return positions.ToArray();
    }
}
