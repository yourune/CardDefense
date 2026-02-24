using BG_Games.Card_Game_Core.Cards;

namespace BG_Games.Card_Game_Core.Systems
{
    public interface IDeckSaveSystem
    {
        public void SaveDeck(DeckData deck);
        public bool LoadDeck(string deckName, out DeckData deck);
        public string[] LoadDeckNames();
        public DeckData[] LoadDecks();
        public void DeleteDeck(string deckName);
    }
}
