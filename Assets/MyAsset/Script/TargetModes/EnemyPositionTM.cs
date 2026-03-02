using UnityEngine;

[System.Serializable]
public class EnemyPositionTM : PositionTargetMode
{
    private Vector3 targetPosition;
    
    /// <summary>
    /// Set the position from a targeted enemy (called by CardView)
    /// </summary>
    public void SetEnemyPosition(EnemyView enemy)
    {
        if (enemy != null)
        {
            targetPosition = enemy.transform.position;
        }
    }
    
    /// <summary>
    /// Set position directly (alternative)
    /// </summary>
    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
    }
    
    public override Vector3[] GetPositions()
    {
        return new Vector3[] { targetPosition };
    }
}
