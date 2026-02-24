using System;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Controllers;

namespace BG_Games.Card_Game_Core.Player.SelectCard
{
    class BotSelectCardHandler:SelectCardHandler
    {
        public override event Action<Card> OnCardSelected;

        public override void Activate()
        {

        }

        public override void Deactivate()
        {
            foreach (var card in NotSelectedCards)
            {
                Destroy(card.gameObject);
            }            
            NotSelectedCards.Clear();
        }

        public override void SelectionPoolDone()
        {
            OnCardSelected?.Invoke(NotSelectedCards.First());
        }

        public override void AddCard(Card card)
        {
            NotSelectedCards.Add(card);
            card.gameObject.SetActive(false);
        }

        public override void RemoveCard(Card card)
        {
            NotSelectedCards.Remove(card);
        }
    }
}
