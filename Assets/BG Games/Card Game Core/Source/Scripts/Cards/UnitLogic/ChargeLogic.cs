using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    class ChargeLogic:UnitCardLogic
    {
        public ChargeLogic(UnitCardInfo info) : base(info)
        {
        }

        public override void SubscribeCardControllerCallbacks(UnitCard controller)
        {
            controller.OnPlaced += ApplyAbillity;
        }

        private void ApplyAbillity()
        {
            ActionAvailable = true;
        }
    }
}
