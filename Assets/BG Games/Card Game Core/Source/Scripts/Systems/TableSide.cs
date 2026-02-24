using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class TableSide : MonoBehaviour,IActionLockable
    {
        [SerializeField] private CardsPlacementGroup _placementGroup;
        [SerializeField] private int _maxCards = 7;

        public event Action<Card> OnPlacedCard;
        public event Action<Card> OnTakeOutCard; 

        private List<Card> _cards = new List<Card>();
        private Collider2D[] _colliders;
        private bool _actionsLocked = false;

        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public ReadOnlyCollection<ITroopCard> TroopCards => (from card in Cards 
                                                          where card is ITroopCard
                                                          select card as ITroopCard).ToList().AsReadOnly();

        private void Start()
        {
            _colliders = GetComponents<Collider2D>();
        }

        public void PlaceCard(Card card)
        {
            if (!CanAddCard())
                return;

            int cardPosition;

            _placementGroup.InsertCard(card, out cardPosition);
            _cards.Insert(cardPosition, card);

            card.CanPlay += CanCardDoMainAction;

            OnPlacedCard?.Invoke(card);
        }

        public void RemoveCard(Card card)
        {
            _placementGroup.RemoveCard(card);
            _cards.Remove(card);

            card.CanPlay -= CanCardDoMainAction;
            OnTakeOutCard?.Invoke(card);
        }

        public async void RemoveCardDelayVisual(Card card, float timeDelay)
        {
            _cards.Remove(card);

            card.CanPlay -= CanCardDoMainAction;
            OnTakeOutCard?.Invoke(card);

            await UniTask.Delay((int)(timeDelay * 1000));
            _placementGroup.RemoveCard(card);
        }
        
        public bool CanAddCardWithPosition(Vector3 position) => IsInsideTableSide2D(position) && CanAddCard();

        public bool CanAddCard() => _cards.Count < _maxCards;

        public bool IsInsideTableSide2D(Vector3 point)
        {
            point.z = transform.position.z;

            foreach (Collider2D collider in _colliders)
            {
                if (collider.bounds.Contains(point))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CanCardDoMainAction(Card card)
        {
            return _actionsLocked == false;
        }


        public void SetLockActions(bool state)
        {
            _actionsLocked = state;
        }

        public void NextTurn()
        {
            foreach (Card card in Cards)
            {
                if (card is ITurnCallbacksListener listener)
                {
                    listener.NextTurn();
                }
            }
        }

        public void EndTurn()
        {
            foreach (Card card in Cards)
            {
                if (card is ITurnCallbacksListener listener)
                {
                    listener.EndTurn();
                }
            }
        }
    }
}
