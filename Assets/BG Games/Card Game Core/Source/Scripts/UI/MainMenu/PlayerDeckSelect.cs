using BG_Games.Card_Game_Core.Cards;
using BG_Games.Card_Game_Core.Systems;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    class PlayerDeckSelect : DeckSelectBase
    {
        private IDeckSaveSystem _deckSaveSystem;
        private DeckSelectUtility _deckSelectUtility;

        [Inject]
        private void Construct(IDeckSaveSystem deckSaveSystem,DeckSelectUtility deckSelectUtility)
        {
            _deckSaveSystem = deckSaveSystem;
            _deckSelectUtility = deckSelectUtility;
        }
        
        public override void SaveChoose()
        {
            base.SaveChoose();
            _deckSelectUtility.SetSelectedDeck(SelectedDeck);
        }

        protected override void UpdateDeckHolders()
        {
            base.UpdateDeckHolders();
            ClearDeckHolders();
            DeckData[] decks = _deckSaveSystem.LoadDecks();

            foreach (DeckData deck in decks)
            {
                if (deck.IsFull())
                {
                    AddDeckSelector(deck);
                }
            }
            
            UpdateView();
        }
    }
}
