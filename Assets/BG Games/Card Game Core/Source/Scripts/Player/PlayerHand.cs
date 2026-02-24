using System;
using System.Collections.ObjectModel;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Visual;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Player
{
    public class PlayerHand : MonoBehaviour,IActionLockable
    {
        [SerializeField] private CardsPlacementGroup _placementGroup;
        [Header("Show Details")]
        [SerializeField] private float _offsetY;
        [SerializeField] private Vector3 _detailedCardScale;

        public event Action<Card> OnCardAdded;
        public event Action<Card> OnCardPlayed;

        private bool _actionsLocked = false;
        private Card _detailedCard;

        private int _maxHandSize;
        private TableSide _tableSide;
        private PlayerEnergy _energy;
        private PlayerDeck _deck; 

        public ReadOnlyCollection<Card> Cards => _placementGroup.Content.AsReadOnly();

        [Inject]
        private void Construct(PlayerId playerId,GameTable table,PlayerEnergy energy, MatchSettings settings, PlayerDeck deck)
        {
            _tableSide = table.GetMyTableSide(playerId);
            _energy = energy;
            _maxHandSize = settings.MaxHandSize;
            _deck = deck;
        }

        public void SetLockActions(bool state)
        {
            _actionsLocked = state;
        }
        
        public void MaxHandRule()
        {
            while (_placementGroup.Content.Count > _maxHandSize)
            {
                Card cardToRemove = _placementGroup.Content[^1];

                _deck.ReturnCardToRandom(cardToRemove.Info);
                RemoveCard(cardToRemove);
                Destroy(cardToRemove.gameObject);
            }
        }

        public void AddCard(Card card)
        {
            HideDetails(_detailedCard);

            card.OnEndPlacing += CardPlaceEnd;
            card.CanPlay += CanPlay;
            _placementGroup.AddCard(card);

            card.OnMouseEnter += ShowDetails;
            card.OnMouseExit += HideDetails;
            OnCardAdded?.Invoke(card);
        }

        private void ShowDetails(Card card)
        {
            if (card != null)
            {
                card.ShowDetailed(transform.position.y + _offsetY, _detailedCardScale);

                _detailedCard = card;
            }
        }

        private void HideDetails(Card card)
        {
            if (card != null)
            {
                card.HideDetailed();
                _placementGroup.RepaintCards();
            }

        }

        public void RemoveCard(Card card)
        {
            card.OnMouseEnter -= ShowDetails;
            card.OnMouseExit -= HideDetails;

            card.OnEndPlacing -= CardPlaceEnd;
            card.CanPlay -= CanPlay;

            _placementGroup.RemoveCard(card);
        }
        
        public bool CanBePlaced(Card card, Vector3 targetPosition)
        {
            if (card.ShouldRemainOnTable)
            {
                return _tableSide.CanAddCardWithPosition(targetPosition);
            }
            else
            {
                return _tableSide.IsInsideTableSide2D(targetPosition);
            }
        }

        public bool CanPlay(Card card)
        {
            return !_actionsLocked && _energy.CanSpend(card.Cost);
        }


        private void CardPlaceEnd(Card card, Vector3 targetPosition)
        {
            if (CanBePlaced(card,targetPosition))
            { 
                CardPlaced(card);
            }
            else
            {
                CardPlaceCancel(card);
            }
        }

        private void CardPlaceCancel(Card card)
        {
            _placementGroup.RepaintCards();
            card.NotPlaced();
        }

        private void CardPlaced(Card card)
        {
            _energy.SpendEnergy(card.Cost);

            RemoveCard(card);            

            card.Placed();
            OnCardPlayed?.Invoke(card);
        }

    }
}
