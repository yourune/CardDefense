using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters
{
    class HeroFilter:ICollectionFilter
    {
        private string _heroID;

        public HeroFilter(string allowedHeroID)
        {
            _heroID = allowedHeroID;
        }

        public bool Filter(CardInfo info)
        {
            if (info is DeckCardInfo)
            {
                DeckCardInfo deckCard = info as DeckCardInfo;

                return deckCard.Hero == null || deckCard.Hero.Id == _heroID;
            }
            else
            {
                return false;
            }
        }
    }
}
