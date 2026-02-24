using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic
{
    [CreateAssetMenu(fileName = "HeroLogic", menuName = "ScriptableObjects/Hero Logic/Damage Enemy")]
    class DamageAbillityHeroFactory:HeroLogicFactory
    {
        [SerializeField]private int _damage = 2;

        public override IHeroLogic CreateLogic(HeroCardInfo info)
        {
            DamageAbillityHero logic = Instantiator.Instantiate<DamageAbillityHero>(new object[] { info, _damage });
            return logic;
        }

        public override ITargetProvider CreateTargetProvider()
        {
            SingleEnemyProvider targetProvider = Instantiator.Instantiate<SingleEnemyProvider>();
            targetProvider.ForbiddenDescriptors = GetDefaultForbiddenDescriptors();
            return targetProvider;
        }
    }
}
