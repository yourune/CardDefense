using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drop multiple rewards simultaneously (for batch processing)
/// </summary>
public class DropMultipleRewardsGA : GameAction
{
    public List<(Vector3 position, int gold, int xp)> Rewards { get; private set; }
    
    public DropMultipleRewardsGA(List<(Vector3 position, int gold, int xp)> rewards)
    {
        Rewards = new List<(Vector3, int, int)>(rewards);
    }
}
