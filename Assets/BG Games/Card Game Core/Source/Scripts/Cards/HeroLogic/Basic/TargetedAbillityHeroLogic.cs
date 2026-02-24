using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic.Basic
{
    public abstract class TargetedAbillityHeroLogic:HeroLogic
    {

        public event Action<Vector3> OnTargetedApply;
        public override event Action OnApply;

        protected ICard _target;

        protected TargetedAbillityHeroLogic(HeroCardInfo info) : base(info)
        {
        }

        protected abstract ICard Action(ITargetProvider targetProvider);

        protected override void AbillityAction(ITargetProvider targetProvider)
        {
            _target = Action(targetProvider);

            OnTargetedApply?.Invoke(_target.Position);
            OnApply?.Invoke();
        }

        protected override void LogAbillityUse()
        {
            BattleLog.LogHeroAbility(HeroInfo, _target.Info, Owner);
        }
    }
}
