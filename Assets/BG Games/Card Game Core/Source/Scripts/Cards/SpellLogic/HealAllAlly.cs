using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    class HealAllAlly:UntargetedSpell
    {
        private int _amount;

        public HealAllAlly(SpellCardInfo info, int amount) : base(info)
        {
            _amount = amount;
        }

        protected override void SpellAction(ITargetProvider targetProvider)
        {
            AllAllyUnitsProvider unitsProvider = targetProvider as AllAllyUnitsProvider;

            if (unitsProvider != null)
            {
                foreach (UnitCard target in unitsProvider.Targets)
                {
                    target.UnitLogic.RestoreHP(_amount);
                }
            }
            else
                throw new Exception("HealAllAlly spell should have AllAllyUnitsProvider");
        }
    }
}
