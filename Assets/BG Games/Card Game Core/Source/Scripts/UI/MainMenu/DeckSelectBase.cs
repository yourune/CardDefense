using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.UI.DeckAssembly;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    abstract class DeckSelectBase : MonoBehaviour
    {
        public Action<DeckData> OnSelectedDeck;
        
        [SerializeField] private Transform _decksContent;
        [SerializeField] private RectTransform _noDecksScreen;
        [SerializeField] private HeroPreview _heroPreview;
        [SerializeField] private TMP_InputField _search;

        private List<DeckSelector> _deckSelectors = new List<DeckSelector>();
        protected IList<HeroCardInfo> Heroes;

        private UniTaskCompletionSource<IList<HeroCardInfo>> _initAsyncCompletionSource;

        protected UniTaskCompletionSource<IList<HeroCardInfo>> InitAsyncCompletionSource
        {
            get
            {
                if (_initAsyncCompletionSource == null)
                {
                    _initAsyncCompletionSource = new UniTaskCompletionSource<IList<HeroCardInfo>>();
                }

                return _initAsyncCompletionSource;
            }
        }


        public bool HasSelected => _lastClicked != null;
        protected DeckData SelectedDeck;
        private DeckSelector _lastClicked;

        private DeckSelectorFactory _factory;
        private CardLoader _cardLoader;

        [Inject]
        private void Construct(DeckSelectorFactory factory,CardLoader cardLoader)
        {
            _factory = factory;
            _cardLoader = cardLoader;
        }

        protected virtual async void Awake()
        {
            _search.onValueChanged.AddListener(Search);
            
            await InitAsync();
            
            InitAsyncCompletionSource.TrySetResult(Heroes);
            UpdateDeckHolders();
        }

        public void UpdateView()
        {
            if (_deckSelectors.Count < 1)
            {
                _noDecksScreen.gameObject.SetActive(true);
            }
            else
            {
                _noDecksScreen.gameObject.SetActive(false);
                _deckSelectors.First()?.HandleClick();
            }
        }
        
        public virtual void SaveChoose() { }

        protected virtual async UniTask InitAsync()
        {
            Heroes = await _cardLoader.LoadHeroes();
        }

        protected virtual void UpdateDeckHolders() { }
        protected virtual void UpdateDeckView(DeckSelector selector, DeckData deck) { }

        protected void AddDeckSelector(DeckData deck)
        {
            DeckSelector selector = _factory.Create(deck, _decksContent);

            selector.OnDeckClicked += DeckClicked;
            selector.OnDeckClicked += SetDeckPreview;
            UpdateDeckView(selector, deck);
            _deckSelectors.Add(selector);
        }

        protected void ClearDeckHolders()
        {
            foreach (Transform deckHolder in _decksContent)
            {
                Destroy(deckHolder.gameObject);
            }

            _deckSelectors = new List<DeckSelector>();
        }

        private void DeckClicked(DeckSelector selector)
        {
            if (_lastClicked != null)
                _lastClicked.Unselect();

            _lastClicked = selector;
        }

        private void SetHeroPreviewData(HeroCardInfo hero)
        {
            _heroPreview.SelectHero(hero);
        }

        private async void SetDeckPreview(DeckSelector selector)
        {
            DeckData deck = selector.Deck;
            SelectedDeck = deck;

            if (Heroes == null)
            {
                await InitAsyncCompletionSource.Task;
            }

            HeroCardInfo deckHero = Heroes.First(hero => hero.Id == deck.Hero);
            SetHeroPreviewData(deckHero);
            OnSelectedDeck?.Invoke(deck);
        }

        private void Search(string keyWord)
        {
            keyWord = keyWord.ToUpper();

            foreach (DeckSelector deckSelector in _deckSelectors)
            {
                if (deckSelector.Deck.Name.ToUpper().Contains(keyWord))
                {
                    deckSelector.gameObject.SetActive(true);
                }
                else
                {
                    deckSelector.gameObject.SetActive(false);
                }
            }
        }
    }
}
