using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Effects;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic
{
    [CreateAssetMenu(fileName = "HeroLogic",menuName = "ScriptableObjects/Hero Logic/Boost Unit")]
    class BoostUnitAbillityHeroFactory:HeroLogicFactory
    {
        [SerializeField] private int _hpBoost;
        [SerializeField] private int _dpBoost;
        [SerializeField] private int _duration;
        [SerializeField] private DurationMode _durationMode;

        public override IHeroLogic CreateLogic(HeroCardInfo info)
        {
            BoostUnitAbillityHero logic = Instantiator.Instantiate<BoostUnitAbillityHero>(new object[] {info,_hpBoost,_dpBoost,_duration,_durationMode});
            return logic;
        }

        public override ITargetProvider CreateTargetProvider()
        {
            SingleAllyUnitProvider targetProvider = Instantiator.Instantiate<SingleAllyUnitProvider>();
            return targetProvider;
        }
    }
}
