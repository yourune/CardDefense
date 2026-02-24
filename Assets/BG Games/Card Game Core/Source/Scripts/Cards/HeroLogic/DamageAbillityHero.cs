using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic
{
    class DamageAbillityHero:TargetedAbillityHeroLogic
    {
        private int _damage;

        public DamageAbillityHero(HeroCardInfo info, int damage) : base(info)
        {
            _damage = damage;
        }

        protected override ICard Action(ITargetProvider targetProvider)
        {
            SingleEnemyProvider enemyProvider = targetProvider as SingleEnemyProvider;

            if (enemyProvider != null)
            {
                ITroopCard target = enemyProvider.Target;
                target.Logic.Attacked(_damage);
                return target;
            }
            else
                throw new Exception("DamageAbillityHero must have SingleEnemyProvider");
        }
    }
}
