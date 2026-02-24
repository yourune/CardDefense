using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    [CreateAssetMenu(fileName = "SpellLogic", menuName = "ScriptableObjects/Spell Logic/Draw Cards")]
    class DrawCardsFactory:SpellLogicFactory
    {
        [SerializeField] private int _drawCardsCount = 2;

        public override ISpellLogic CreateLogic(SpellCardInfo info)
        {
            DrawCards logic = Instantiator.Instantiate<DrawCards>(new object[] {info, _drawCardsCount });
            return logic;
        }

        public override ITargetProvider CreateTargetProvider()
        {
            MockTargetProvider targetProvider = Instantiator.Instantiate<MockTargetProvider>();
            return targetProvider;
        }
    }
}
