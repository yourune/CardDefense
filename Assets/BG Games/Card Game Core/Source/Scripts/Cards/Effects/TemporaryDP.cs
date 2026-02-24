using System;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;

namespace BG_Games.Card_Game_Core.Cards.Effects
{
    public class TemporaryDP : TemporaryEffect
    {
        private int _amount;

        private IUnitLogic _targetUnit;

        public TemporaryDP(int amount)
        {
            _amount = amount;
        }

        protected override void ApplyEffect()
        {
            if (Target is IUnitLogic)
            {
                _targetUnit = Target as IUnitLogic;
                _targetUnit.GainDP(_amount);
            }
            else
            {
                throw new Exception("Target of temporary dp effect must implement IUnitLogic interface");
            }
        }

        public override void Remove()
        {
            _targetUnit.RemoveDP(_amount);            
            base.Remove();
        }
    }
}
