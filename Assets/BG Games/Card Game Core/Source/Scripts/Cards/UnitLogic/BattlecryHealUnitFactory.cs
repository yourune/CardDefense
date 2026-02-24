using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    [CreateAssetMenu(fileName = "Logic", menuName = "ScriptableObjects/Unit Logic/Battlecry Heal Unit")]
    class BattlecryHealUnitFactory:UnitLogicFactory,ISFXInfo
    {
        [SerializeField] private int _healAmount = 3;
        [SerializeField] private AudioClip _sfx;

        public AudioClip SFX => _sfx;

        public override IUnitLogic CreateLogic(UnitCardInfo info)
        {
            SingleAllyProvider singleAllyUnitProvider = Instantiator.Instantiate<SingleAllyProvider>();

            BattlecryHealUnit logic = Instantiator.Instantiate<BattlecryHealUnit>(new object[] {info,_healAmount,singleAllyUnitProvider});
            return logic;
        }
    }
}
