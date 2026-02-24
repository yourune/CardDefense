using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    class DamageEnemyAOE:TargetedSpell
    {
        private int _primaryDamage;
        private int _nearbyDamage;

        public DamageEnemyAOE(SpellCardInfo info, int primaryDamage, int nearbyDamage):base(info)
        {
            _primaryDamage = primaryDamage;
            _nearbyDamage = nearbyDamage;
        }

        protected override ICard SpellAction(ITargetProvider targetProvider)
        {
            EnemyAndAdjacentProvider concreteProvider = targetProvider as EnemyAndAdjacentProvider;

            if (concreteProvider != null)
            {
                concreteProvider.PrimaryTarget.Logic.Attacked(_primaryDamage);

                foreach (ITroopCard target in concreteProvider.AdjacentTargets)
                {
                    target.Logic.Attacked(_nearbyDamage);
                }       
                
                return concreteProvider.PrimaryTarget;
            }
            else
                throw new Exception("TacticalBombardment spell uses EnemyAndAdjacentProvider");
        }
    }
}
