using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Player.Mulligan
{
    public abstract class MulliganHandler:MonoBehaviour
    {
        protected abstract IEnumerable<Card> StartHand { get; }

        protected PlayerHand PlayerHand;

        [Inject]
        private void Construct(PlayerHand playerHand)
        {
            PlayerHand = playerHand;
        }

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void SelectionPoolDone();
        public abstract void AddCard(Card card);

        public virtual void AddCardsToStartHand()
        {
            foreach (var card in StartHand)
            {
                PlayerHand.AddCard(card);
            }
        }
    }
}
