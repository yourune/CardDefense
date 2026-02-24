using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    [CreateAssetMenu(fileName = "Unit_logic",menuName = "ScriptableObjects/Unit Logic/Charge")]
    class ChargeLogicFactory:UnitLogicFactory
    {
        public override IUnitLogic CreateLogic(UnitCardInfo info)
        {
            ChargeLogic logic = Instantiator.Instantiate<ChargeLogic>(new object[] { info });
            return logic;
        }
    }
}
