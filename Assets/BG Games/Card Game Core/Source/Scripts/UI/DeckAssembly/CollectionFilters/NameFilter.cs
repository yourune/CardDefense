using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters
{
    class NameFilter:ICollectionFilter
    {
        public string KeyWord { get; set; }
        
        public bool Filter(CardInfo info)
        {
            if (string.IsNullOrEmpty(KeyWord))
            {
                return true;
            }
            else
            {
                string nameLower = info.Name.GetLocalizedStringAsync().Result.ToLower();
                string keyLower = KeyWord.ToLower();

                return nameLower.Contains(keyLower);
            }
        }
    }
}
