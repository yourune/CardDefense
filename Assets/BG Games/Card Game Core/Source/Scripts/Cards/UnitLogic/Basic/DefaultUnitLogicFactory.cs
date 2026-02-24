using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic.Basic
{
    [CreateAssetMenu(fileName = "Unit Logic", menuName = "ScriptableObjects/Unit Logic/Default Unit")]
    public class DefaultUnitLogicFactory:UnitLogicFactory
    {
        public override IUnitLogic CreateLogic(UnitCardInfo info)
        {
            DefaultUnitLogic unitLogic = Instantiator.Instantiate<DefaultUnitLogic>(new object[] {info});
            return unitLogic;
        }
    }
}
