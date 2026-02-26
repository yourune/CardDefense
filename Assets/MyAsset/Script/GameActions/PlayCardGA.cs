using UnityEngine;

public class PlayCardGA : GameAction
{
    public EnemyView ManualTarget { get; private set; } 
    public Cards Cards { get; private set; }
    public PlayCardGA(Cards cards)
    {
        Cards = cards;
        ManualTarget = null;
    }
    public PlayCardGA(Cards cards, EnemyView manualTarget)
    {
        Cards = cards;
        ManualTarget = manualTarget;
    }
}
