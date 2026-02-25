using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction
{
    public int DamageAmount { get; private set; }
    public List<CombatantView> Targets { get; private set; }
    public DealDamageGA(int damageAmount, List<CombatantView> targets)
    {
        DamageAmount = damageAmount;
        Targets = new(targets);
    }
}
