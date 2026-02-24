using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    class DamageEnemy:TargetedSpell
    {
        private int _damage;

        public DamageEnemy(SpellCardInfo info, int damage):base(info)
        {
            _damage = damage;
        }

        protected override ICard SpellAction(ITargetProvider targetProvider)
        {
            SingleEnemyProvider unitProvider = targetProvider as SingleEnemyProvider;
            if (unitProvider != null)
            {
                DealDamage(unitProvider.Target.Logic);
                return unitProvider.Target;
            }
            else
                throw new Exception("DealDamageToUnit spell should have SingleEnemyUnitProvider");
        }

        private void DealDamage(ITroopLogic target)
        {
            target.Attacked(_damage);
        }
    }
}
