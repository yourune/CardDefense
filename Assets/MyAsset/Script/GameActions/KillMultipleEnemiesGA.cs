using System.Collections.Generic;

/// <summary>
/// Kill multiple enemies simultaneously (for batch processing)
/// </summary>
public class KillMultipleEnemiesGA : GameAction
{
    public List<EnemyView> Enemies { get; private set; }
    
    public KillMultipleEnemiesGA(List<EnemyView> enemies)
    {
        Enemies = new List<EnemyView>(enemies); // Copy to avoid modification
    }
}
