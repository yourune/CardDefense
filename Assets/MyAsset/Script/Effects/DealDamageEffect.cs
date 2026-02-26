using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;
    public override GameAction GetGameAction(List<CombatantView> targets)
    {
        DealDamageGA dealDamageGA = new(damageAmount, targets);
        return dealDamageGA;
    }
}
