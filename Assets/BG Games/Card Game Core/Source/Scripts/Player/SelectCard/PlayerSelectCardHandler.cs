using System;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Visual;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Player.SelectCard
{
    class PlayerSelectCardHandler:SelectCardHandler
    {
        [SerializeField] private CardsPlacementGroup _placementGroup;
        [Space]
        [SerializeField] private RectTransform _screen;
        
        private bool _activated;

        public override event Action<Card> OnCardSelected;

        public override void Activate()
        {
            _activated = true;
            _screen.gameObject.SetActive(true);

            foreach (var card in _placementGroup.Content)
            {
                SubscribeCard(card);
            }
        }

        public override void Deactivate()
        {
            _activated = false;
            _screen.gameObject.SetActive(false);

            foreach (var card in _placementGroup.Content)
            {
                UnsubscribeCard(card);
                Destroy(card.gameObject);
            }

            _placementGroup.Content.Clear();
            NotSelectedCards.Clear();
        }

        public override void SelectionPoolDone()
        {
            
        }

        public override void AddCard(Card card)
        {
            _placementGroup.AddCard(card);
            NotSelectedCards.Add(card);

            if (_activated)
            {
                SubscribeCard(card);
            }
        }

        public override void RemoveCard(Card card)
        {
            _placementGroup.RemoveCard(card);
            NotSelectedCards.Remove(card);
        }

        private bool BlockPlayingCard(Card card) => false;

        private void SubscribeCard(Card card)
        {
            card.SetToTopLayer();
            card.SetEnabledInputReading(true);
            card.CanPlay += BlockPlayingCard;
            card.OnClick += Select;
        }
        private void UnsubscribeCard(Card card)
        {
            card.SetToDefaultLayer();
            card.CanPlay -= BlockPlayingCard;
            card.OnClick -= Select;
        }

        private void Select(Card card)
        {
            SelectedCard = card;
            NotSelectedCards.Remove(card);
            OnCardSelected?.Invoke(card);
        }
    }
}
