using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;

namespace BG_Games.Card_Game_Core.Player.Mulligan
{
    class BotMulliganHandler:MulliganHandler
    {
        private List<Card> _startHand = new List<Card>();

        protected override IEnumerable<Card> StartHand => _startHand;

        public override void Activate()
        {

        }

        public override void Deactivate()
        {

        }

        public override void SelectionPoolDone()
        {
            
        }

        public override void AddCard(Card card)
        {
            _startHand.Add(card);
            card.gameObject.SetActive(false);
        }

        public override void AddCardsToStartHand()
        {
            foreach (var card in StartHand)
            {
                card.gameObject.SetActive(true);
                PlayerHand.AddCard(card);
            }
        }
    }
}
