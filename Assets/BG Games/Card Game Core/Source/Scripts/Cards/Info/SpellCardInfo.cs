using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Info
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Info/Spell")]
    public class SpellCardInfo:DeckCardInfo
    {
        [field: Space]
        [field: SerializeField] public SpellLogicFactory LogicFactory { get; private set; }        
        [field:SerializeField] public AudioClip SFX { get; private set; }

        public ISpellLogic GetLogic()
        {
            return LogicFactory.CreateLogic(this);
        }

        public ITargetProvider GetTargetProvider()
        {
            return LogicFactory.CreateTargetProvider();
        }
    }
}
