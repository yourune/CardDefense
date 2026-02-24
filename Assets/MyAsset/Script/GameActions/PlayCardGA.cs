using UnityEngine;

public class PlayCardGA : GameAction
{
    public Cards Cards { get; private set; }
    public PlayCardGA(Cards cards)
    {
        Cards = cards;
    }
}
