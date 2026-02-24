using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    class StealthLogic:UnitCardLogic
    {
        public StealthLogic(UnitCardInfo info) : base(info)
        {
        }

        public override void SubscribeCardControllerCallbacks(UnitCard controller)
        {
            controller.OnPlaced += ApplyAbillity;
        }

        private void ApplyAbillity()
        {
            this.Descriptors.Add(new StealthDescriptor());
            Debug.Log("Added stealth descriptor to " + this.ToString());
        }

        public override void NextTurn()
        {
            base.NextTurn();

            (this as ILogicWithDescriptors).RemoveDescriptor<StealthDescriptor>();
            Debug.Log("Removed stealth descriptor from " + this.ToString());
        }
    }
}
