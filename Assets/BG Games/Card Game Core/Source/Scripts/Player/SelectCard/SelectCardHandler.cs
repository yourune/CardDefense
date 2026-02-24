using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Player.SelectCard
{
    abstract class SelectCardHandler:MonoBehaviour
    {
        protected Card SelectedCard;

        public List<Card> NotSelectedCards { get; protected set; } = new List<Card>();

        public abstract event Action<Card> OnCardSelected;

        public virtual void SetCardsGroupPosition(Vector3 position)
        {

        }
        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void SelectionPoolDone();
        public abstract void AddCard(Card card);
        public abstract void RemoveCard(Card card);
    }
}
