using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    [CreateAssetMenu(fileName = "SpellLogic", menuName = "ScriptableObjects/Spell Logic/Heal All Ally")]
    class HealAllAllyFactory:SpellLogicFactory
    {
        [SerializeField] private int _amount = 1;

        public override ISpellLogic CreateLogic(SpellCardInfo info)
        {
            HealAllAlly logic = Instantiator.Instantiate<HealAllAlly>(new object[] { info, _amount });
            return logic;
        }

        public override ITargetProvider CreateTargetProvider()
        {
            AllAllyUnitsProvider targetProvider = Instantiator.Instantiate<AllAllyUnitsProvider>();
            return targetProvider;
        }
    }
}
