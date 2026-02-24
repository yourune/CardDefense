using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters
{
    class UnitFilter:ICollectionFilter
    {
        public bool Filter(CardInfo info)
        {
            return info is UnitCardInfo;
        }
    }
}
