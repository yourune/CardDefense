using UnityEngine;

public class AttackCastleGA : GameAction
{
    public CastleView Castle { get; private set; }
    public int Damage { get; private set; }

    public AttackCastleGA(CastleView castle, int damage = 1)
    {
        Castle = castle;
        Damage = damage;
    }
}
