using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters
{
    public interface ICollectionFilter
    {
        public bool Filter(CardInfo info);
    }
}
