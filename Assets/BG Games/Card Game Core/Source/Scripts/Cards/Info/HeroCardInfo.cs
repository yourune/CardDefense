using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.HeroLogic.Basic;
using UnityEngine;
using UnityEngine.Localization;

namespace BG_Games.Card_Game_Core.Cards.Info
{
    [CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjects/Info/Hero")]
    public class HeroCardInfo:CardInfo,ICardTroopInfo
    {
        public CardInfo CardInfo => this;

        [field: Space,SerializeField] public LocalizedString Allias { get; private set; }
        [field: SerializeField] public LocalizedString AbillityName { get; private set; }
        [field: SerializeField] public int HP { get; private set; }

        [field: Space]
        [field: SerializeField] public HeroLogicFactory LogicFactory { get; set; }
        [field: SerializeField] public AudioClip SFX { get; private set; }
        
        [field: Header("Hero AI Ability")] 
        [field: SerializeField] [field: Range(0, 1)] public float BeforePlacingProbability { get; private set; } = 0.33f;
        [field: SerializeField] [field: Range(0, 1)] public float BeforeAttackingProbability { get; private set; } = 0.33f;
        [field: SerializeField] [field: Range(0, 1)] public float AfterAttackingProbability { get; private set; } = 0.33f;

        public IHeroLogic GetCardLogic()
        {
            return LogicFactory.CreateLogic(this);
        }

        public ITargetProvider GetTargetProvider()
        {
            return LogicFactory.CreateTargetProvider();
        }
    }
}
