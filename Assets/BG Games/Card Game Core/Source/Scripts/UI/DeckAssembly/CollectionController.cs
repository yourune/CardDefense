using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.SaveSystem;
using BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Factories;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    public class CollectionController : DynamicCardsContainer
    {
        [SerializeField] private LayoutGroup _layoutGroup;
        [SerializeField] private CollectionFiltersController _filtersController;
        [SerializeField] private CollectionSearchController _searchController;

        public List<ICollectionFilter> Filters { get; private set; } = new List<ICollectionFilter>();

        private ICollectionFilter _searchFilter;
        private ICollectionFilter _heroFilter;
        private ICollectionFilter _inventoryFilter;

        private bool _draggableCards = false;

        private List<DeckCardController> _collection = new List<DeckCardController>();

        protected override LayoutGroup ContentHolder => _layoutGroup;

        private CardControllerFactory _factory;
        private CardLoader _cardLoader;
        private DeckCreatorController _deckCreator;
        private IPlayerInventorySaveSystem _playerInventorySaveSystem;

        [Inject]
        public void Construct(CardControllerFactory cardFactory,CardLoader cardLoader,DeckCreatorController deckCreator,
            IPlayerInventorySaveSystem playerInventorySaveSystem)
        {
            _factory = cardFactory;
            _cardLoader = cardLoader;
            _deckCreator = deckCreator;
            _playerInventorySaveSystem = playerInventorySaveSystem;
        }

        private async void Start()
        {
            IList<CardInfo> cards = await _cardLoader.LoadAllCards();
            cards = cards.OrderBy(info => info.Cost).ToList();

            foreach (var info in cards)
            {
                DeckCardController instance = _factory.Create(info, _layoutGroup.transform);
                instance.Draggable = false;

                instance.OnDragStarted += card => TakeCard(card.CardInfoHolder);
                instance.OnDragEnded += EndCardDrag;

                _collection.Add(instance);
            }
        }

        public void SetTypeFilters(List<ICollectionFilter> filters)
        {
            Filters = filters;
            UpdateView();
        }

        public void SetSearchFilter(ICollectionFilter filter)
        {
            _searchFilter = filter;
            UpdateView();
        }

        public void EndDeckAssembly()
        {
            _filtersController.ResetFilters();
            _heroFilter = null;
            _inventoryFilter = null;
            _draggableCards = false;
            UpdateView();
        }

        public void DeckAssemblyStarted(DeckData deck)
        {
            _heroFilter = new HeroFilter(deck.Hero);
            _inventoryFilter = new InventoryFilter(_playerInventorySaveSystem.Load());
            _filtersController.ResetFilters();
            _searchController.ResetSearch();
            _draggableCards = true;
            UpdateView();
        }

        public void UpdateView()
        {
            foreach (DeckCardController card in _collection)
            {
                bool approved = true;

                if (_heroFilter != null && !_heroFilter.Filter(card.Info))
                {
                    approved = false;
                }
                else if (_searchFilter != null && !_searchFilter.Filter(card.Info))
                {
                    approved = false;
                }
                else if (_inventoryFilter != null && !_inventoryFilter.Filter(card.Info))
                {
                    approved = false;
                }
                else
                {
                    foreach (ICollectionFilter filter in Filters)
                    {
                        if (!filter.Filter(card.Info))
                        {
                            approved = false;
                            break;
                        }
                    }
                }

                card.Draggable = _draggableCards && approved;
                card.gameObject.SetActive(approved);
            }
        }

        private void EndCardDrag(CardController card)
        {
            ReturnCard();
            if (_deckCreator.IsPointInside(card.CardInfoHolder.transform.position))
            {
                _deckCreator.AddCard(card.CardInfoHolder.Info);
            }
        }


    }
}