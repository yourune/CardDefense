using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters
{
    class RaceFilter:ICollectionFilter
    {
        private CardRace _race;

        public RaceFilter(CardRace race)
        {
            _race = race;
        }

        public bool Filter(CardInfo info)
        {
            return info.Race == _race;
        }
    }
}
