using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    [CreateAssetMenu(fileName = "Unit_logic",menuName = "ScriptableObjects/Unit Logic/Stealth")]
    class StealthLogicFactory:UnitLogicFactory
    {
        public override IUnitLogic CreateLogic(UnitCardInfo info)
        {
            StealthLogic logic = Instantiator.Instantiate<StealthLogic>(new object[]{info});
            return logic;
        }
    }
}
