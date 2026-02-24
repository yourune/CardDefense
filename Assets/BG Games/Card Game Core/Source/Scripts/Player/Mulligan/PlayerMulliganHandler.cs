using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Visual;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.Player.Mulligan
{
    class PlayerMulliganHandler:MulliganHandler
    {
        [SerializeField] private CardsPlacementGroup _placementGroup;
        [SerializeField] private Transform _crossOutPrefab;
        [Space] 
        [SerializeField] private RectTransform _screen;
        [SerializeField] private Button _confirmMulligan;

        private List<Card> _wastedCards = new List<Card>();
        private Dictionary<Card, Transform> _crossOuts = new Dictionary<Card, Transform>();

        private bool _activated;
        private bool _mulliganPerformed;

        private DrawCard _drawCard;
        private PlayerDeck _deck;

        [Inject]
        private void Construct(DrawCard drawCard,PlayerDeck deck)
        {
            _drawCard = drawCard;
            _deck = deck;
        }

        private void Awake()
        {
            _confirmMulligan.onClick.AddListener(ConfirmMulligan);
        }
        
        protected override IEnumerable<Card> StartHand => _placementGroup.Content;

        public override void SelectionPoolDone()
        {
            
        }

        public override void AddCard(Card card) 
        {            
            _placementGroup.AddCard(card);
            if (_activated)
            {
                SubscribeCard(card);
            }

        }

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
            }

            ClearMarks();
        }

        public override void AddCardsToStartHand()
        {
            foreach (Card card in StartHand.ToList())
            {
                _placementGroup.RemoveCard(card);
                PlayerHand.AddCard(card);
            }
        }


        private void RemoveCard(Card card)
        {
            _placementGroup.RemoveCard(card);
            _deck.ReturnCardToRandom(card.Info);
        }

        private void ChangeWasteState(Card card)
        {
            if (_mulliganPerformed)
                return;

            if (_wastedCards.Contains(card) == false)
            {
                WasteCard(card);
            }
            else
            {
                UnwasteCard(card);
            }
        }

        private async void ConfirmMulligan()
        {
            if (_mulliganPerformed)
                return;

            _confirmMulligan.gameObject.SetActive(false);

            int wastedCardsCount = _wastedCards.Count;

            foreach (var card in _wastedCards)
            {
                RemoveCard(card);
                Destroy(card.gameObject);
            }
            ClearMarks();

            if (wastedCardsCount > 0)
            {
                Card[] cards = await _drawCard.DrawCards(wastedCardsCount);

                foreach (Card card in cards)
                {
                    AddCard(card);
                }
            }
        }

        private void WasteCard(Card card)
        {
            Transform crossOut = Instantiate(_crossOutPrefab).transform;
            crossOut.position = card.transform.position;

            _wastedCards.Add(card);
            _crossOuts.Add(card, crossOut);
        }

        private void UnwasteCard(Card card)
        {
            Transform crossOut = _crossOuts[card];
            _crossOuts.Remove(card);
            _wastedCards.Remove(card);
            Destroy(crossOut.gameObject);
        }

        private bool BlockPlayingCard(Card card) => false;

        private void SubscribeCard(Card card)
        {
            card.SetToTopLayer();
            card.SetEnabledInputReading(true);
            card.CanPlay += BlockPlayingCard;
            card.OnClick += ChangeWasteState;
        }

        private void UnsubscribeCard(Card card)
        {
            card.SetToDefaultLayer();
            card.CanPlay -= BlockPlayingCard;
            card.OnClick -= ChangeWasteState;
        }

        private void ClearMarks()
        {
            foreach (var crossOut in _crossOuts)
            {
                Destroy(crossOut.Value.gameObject);
            }

            _wastedCards.Clear();
            _crossOuts.Clear();

            _mulliganPerformed = true;
        }
    }
}
