using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.Audio;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Factories;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    public class DeckCreatorController : DynamicCardsContainer
    {
        [SerializeField] private RectTransform _screen;
        [SerializeField] private LayoutGroup _layoutGroup;
        [SerializeField] private TMP_InputField _deckName;
        [SerializeField] private TMP_Text _deckFullness;
        [SerializeField] private Button _save;
        [Space]        
        [SerializeField] private AudioClip _addCardSound;
        [SerializeField] private AudioClip _deleteCardSound;
        [SerializeField] private AudioClip _unaccesSound;

        private AudioSystem _audioSystem;


        private List<CardBarController> _cardBars = new List<CardBarController>();
        private RectTransform _rectTransform;
        private DeckData _deck;

        protected override LayoutGroup ContentHolder => _layoutGroup;

        private CardBarControllerFactory _factory;
        private IDeckSaveSystem _deckSaveSystem;
        private CardLoader _cardLoader;
        private DeckAssemblyNavigation _assemblyNavigation;

        [Inject]
        private void Construct(CardBarControllerFactory factory, CardLoader cardLoader,IDeckSaveSystem deckSaveSystem,DeckAssemblyNavigation assemblyNavigation,AudioSystem audioSystem)
        {
            _factory = factory;
            _cardLoader = cardLoader;
            _deckSaveSystem = deckSaveSystem;
            _assemblyNavigation = assemblyNavigation;
            _audioSystem = audioSystem;
            
            _rectTransform = transform as RectTransform;
        }

        private void Awake()
        {
            _save.onClick.AddListener(SaveDeck);
        }

        public void Open(DeckData deck)
        {
            _screen.gameObject.SetActive(true);
            SetDeckAsync(deck);
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
            ClearCardBars();
        }

        public void AddCard(CardInfo card)
        {
            int duplicates;

            if (UpdateDeckWithAdd(card,out duplicates))
            {
                AddCardBar(card,duplicates);

                _audioSystem.PlayUI(_addCardSound);
            }
            else
            {

                _audioSystem.PlayUI(_unaccesSound);
            }

        }

        public bool IsPointInside(Vector3 point)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, point, null, out localPoint);

            return _rectTransform.rect.Contains(localPoint);
        }

        public override void ReturnCard()
        {
            if (!IsPointInside(TakenCard.Transform.position))
            {
                bool removedDuplicate;

                RemoveCard(TakenCard, out removedDuplicate);
                ContentHolder.enabled = true;

                if (removedDuplicate)
                {
                    base.ReturnCard();
                }

                _audioSystem.PlayUI(_deleteCardSound);
            }
            else
            {
                base.ReturnCard();
            }
        }

        private async void SetDeckAsync(DeckData deck)
        {
            _deck = deck;
            _deckName.text = _deck.Name;

            IList<CardInfo> cards = await _cardLoader.LoadDeckCards(_deck);

            foreach (var cardId in _deck.Cards)
            {
                if (!string.IsNullOrEmpty(cardId))
                {
                    CardInfo card = cards.First(info => info.Id == cardId);
                    int duplicates = _deck.CountDuplicates(cardId);
                    AddCardBar(card, duplicates);
                }
            }
            _deckFullness.text = DeckData.GetFullnesAsString(_deck);
        }

        private void ClearCardBars()
        {
            _cardBars = new List<CardBarController>();
            foreach (Transform cardBar in _layoutGroup.transform)
            {
                Destroy(cardBar.gameObject);
            }
        }

        private void AddCardBar(CardInfo card,int duplicates)
        {
            if (UpdateCardCount(card.Id,duplicates) == false)
            {
                CardBarController createdBar = _factory.Create(card, _layoutGroup.transform);
                _cardBars.Add(createdBar);
            }
        }

        private bool UpdateDeckWithAdd(CardInfo newCard,out int duplicates)
        {
            if (_deck.AddCard(newCard.Id,out duplicates))
            {
                _deckFullness.text = DeckData.GetFullnesAsString(_deck);
                return true;
            }
            else
            {
                return false;
            }

        }

        private void UpdateDeckWithRemove(CardInfo card)
        {
            int index =_deck.Cards.IndexOf(card.Id);
            _deck.Cards[index] = null;
             
            _deckFullness.text = DeckData.GetFullnesAsString(_deck);
        }

        private void RemoveCard(ICardInfoHolder card,out bool removedDuplicate)
        {
            int duplicates = _deck.CountDuplicates(card.Info.Id);
            if (duplicates > 1)
            {
                UpdateCardCount(card.Info.Id,duplicates-1);
                removedDuplicate = true;
            }
            else
            {                
                CardBarController cardBar = _cardBars.Find(card => card.CardId == TakenCard.Info.Id);
                _cardBars.Remove(cardBar);

                removedDuplicate = false;
                Destroy(TakenCard.Transform.gameObject);
            }

            UpdateDeckWithRemove(card.Info);
        }

        private bool UpdateCardCount(string id,int duplicates)
        {
            CardBarController cardBar = _cardBars.FirstOrDefault(bar => bar.CardId == id);
            if (cardBar != null)
            {
                cardBar.SetCountOfDuplicates(duplicates);
                return true;
            }

            return false;
        }

        private void SaveDeck()
        {
            if (_deck.Name != _deckName.text)
            {                
                _deckSaveSystem.DeleteDeck(_deck.Name);
                _deck.Name = _deckName.text;
            }
            _deckSaveSystem.SaveDeck(_deck);
            _assemblyNavigation.OpenDeckSelect();
        }
    }
}