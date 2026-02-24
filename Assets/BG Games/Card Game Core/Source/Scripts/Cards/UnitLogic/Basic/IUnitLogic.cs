using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic.Basic
{
    public interface IUnitLogic:ITroopLogic
    {
        public event Action<int> OnDPChanged;
        public event Action<Vector3> OnAttack;
        public event Action<int> OnCounterAttacked;

        public bool ActionAvailable { get; }
        public int DP { get; }
        
        public void DoMainAction(ITargetProvider targetProvider);

        public void SubscribeCardControllerCallbacks(UnitCard controller);

        public void ResetCard();

        public void GainDP(int amount);
        public void RemoveDP(int amount);

        public void Counterattacked(int amount, IUnitLogic enemy);

        public bool WillCounterattack(ITroopLogic attacker);
        public bool WillBeKilled(int amount, ITroopLogic enemy);
        public int CalculateCounterattackDamage(ITroopLogic attacker);
    }
}
