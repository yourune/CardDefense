using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.Localization;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Factories;
using BG_Games.Card_Game_Core.UI.DeckAssembly.Items;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    public class DeckSelect : MonoBehaviour
    {
        [SerializeField] private Button _newDeck;
        [SerializeField] private DialogWindow _deleteConfirmWindow;
        [SerializeField] private RectTransform _screen;
        [SerializeField] private RectTransform _decksTransfom;
        [SerializeField] private DeckSettings _deckSettings;

        private DeckData[] _decks;
        private IDeckSaveSystem _deckSaveSystem;
        private DeckNameHolderFactory _factory;
        private DeckAssemblyNavigation _assemblyNavigation;
        private HeroSelect _heroSelect;

        [Inject]
        private void Construct(IDeckSaveSystem deckSaveSystem, DeckNameHolderFactory factory, DeckAssemblyNavigation deckAssemblyNavigation,HeroSelect heroSelect)
        {
            _deckSaveSystem = deckSaveSystem;
            _factory = factory;
            _assemblyNavigation = deckAssemblyNavigation;
            _heroSelect = heroSelect;
        }

        private void Awake()
        {
            _newDeck.onClick.AddListener(StartCreatingDeck);
            UpdateDeckHolders();
        }

        private void AddDeckHolder(DeckData deck)
        {
            DeckHolder deckHolder = _factory.Create(deck, _decksTransfom);
            deckHolder.OnDeckSelected += OpenDeck;
            deckHolder.OnDeckDelete += DeleteDeckClick;
        }

        private DeckData _selectedToDelete;

        private void DeleteDeckClick(DeckData deck)
        {
            _deleteConfirmWindow.Open();

            _selectedToDelete = deck;

            _deleteConfirmWindow.OnAccept += DeleteDeck;
            _deleteConfirmWindow.OnCancel += ClearDeleteSelection;
            _deleteConfirmWindow.OnClose += ClearDeleteSelection;
        }

        private void ClearDeleteSelection()
        {
            _selectedToDelete = default(DeckData);
            _deleteConfirmWindow.OnAccept -= DeleteDeck;
            _deleteConfirmWindow.OnCancel -= ClearDeleteSelection;
            _deleteConfirmWindow.OnClose -= ClearDeleteSelection;

            _deleteConfirmWindow.Close();
        }

        private void DeleteDeck()
        {
            _deckSaveSystem.DeleteDeck(_selectedToDelete.Name);
            UpdateDeckHolders();

            ClearDeleteSelection();
        }
        

        private void ClearDeckHolders()
        {
            foreach (Transform deckHolder in _decksTransfom)
            {
                Destroy(deckHolder.gameObject);
            }
        }

        private void UpdateDeckHolders()
        {
            ClearDeckHolders();
            _decks = _deckSaveSystem.LoadDecks();

            foreach (DeckData deck in _decks)
            {
                AddDeckHolder(deck);
            }
        }

        private void OpenDeck(DeckData deck)
        {
            _assemblyNavigation.OpenDeck(deck);
        }

        private void StartCreatingDeck()
        {
            _assemblyNavigation.CloseAll();
            _heroSelect.Open(hero =>
            {
                CreateDeck(hero);
                _assemblyNavigation.OpenAll();
            }, () =>
            {
                _assemblyNavigation.OpenAll();
            });
        }

        private void CreateDeck(CardInfo hero)
        {
            var defaultDeckName = LocalizationData
                .GetLocalized(LocalizationData.KeyPrefixes.LabelDeck, LocalizationData.UILocalizationTable);
            DeckData deck = new DeckData($"{defaultDeckName} {_decks.Length+1}", hero.Id,hero.Race, _deckSettings.Size, _deckSettings.MaxDuplicates);
            _deckSaveSystem.SaveDeck(deck);

            _assemblyNavigation.OpenDeck(deck);
        }

        public void Open()
        {
            _screen.gameObject.SetActive(true);
            UpdateDeckHolders();
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
        }
    }
}