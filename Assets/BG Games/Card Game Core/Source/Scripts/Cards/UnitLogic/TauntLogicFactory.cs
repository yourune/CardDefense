using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    [CreateAssetMenu(fileName = "UnitLogic",menuName = "ScriptableObjects/Unit Logic/Taunt")]
    class TauntLogicFactory:UnitLogicFactory
    {
        public override IUnitLogic CreateLogic(UnitCardInfo info)
        {
            TauntLogic instance = Instantiator.Instantiate<TauntLogic>(new object[] { info });
            return instance;
        }
    }
}
