using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Info
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Info/Unit")]
    public class UnitCardInfo:DeckCardInfo,ICardTroopInfo
    {
        public CardInfo CardInfo => this;

        [field: SerializeField] public int DP { get; private set; }
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public UnitLogicFactory LogicFactory { get; private set; }

        public ITargetProvider GetTargetProvider()
        {
            return LogicFactory.CreateTargetProvider();
        }

        public IUnitLogic GetCardLogic()
        {
            return LogicFactory.CreateLogic(this);
        }
    }
}
