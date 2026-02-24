using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic.Basic
{
    public abstract class UntargetedAbillityHeroLogic:HeroLogic
    {
        public override event Action OnApply;

        protected UntargetedAbillityHeroLogic(HeroCardInfo info) : base(info)
        {
        }

        protected abstract void Action(ITargetProvider targetProvider);

        protected override void AbillityAction(ITargetProvider targetProvider)
        {
            Action(targetProvider);

            OnApply?.Invoke();
        }
    }
}
