using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Effects;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic
{
    class BoostUnitAbillityHero:TargetedAbillityHeroLogic
    {
        private int _hpBoost;
        private int _dpBoost;
        private int _duration;
        private DurationMode _effectsDurationMode;


        public BoostUnitAbillityHero(HeroCardInfo info, int hpBoost, int dpBoost, int duration, DurationMode durationMode) : base(info)
        {
            _hpBoost = hpBoost;
            _dpBoost = dpBoost;
            _duration = duration;
            _effectsDurationMode = durationMode;
        }

        protected override ICard Action(ITargetProvider targetProvider)
        {
            SingleAllyUnitProvider unitProvider = targetProvider as SingleAllyUnitProvider;

            if (unitProvider != null)
            {
                UnitCard target = unitProvider.Target;
                ApplyEffects(target.UnitLogic);
                return target;
            }
            else
                throw new Exception($"{nameof(BoostUnitAbillityHero)} uses {nameof(SingleAllyProvider)}");
        }

        private void ApplyEffects(IUnitLogic target)
        {
            TemporaryDP _boostDp = new TemporaryDP(_dpBoost);
            TemporaryHP _boostHp = new TemporaryHP(_hpBoost);

            _boostDp.Apply(target, _duration, _effectsDurationMode);
            _boostHp.Apply(target, _duration, _effectsDurationMode);
        }
    }
}
