using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly.CollectionFilters
{
    public class InventoryFilter : ICollectionFilter
    {
        private InventoryData _inventoryData;

        public InventoryFilter(InventoryData data)
        {
            _inventoryData = data;
        }
        
        public bool Filter(CardInfo info)
        {
            return _inventoryData.Cards.Contains(info.Id);
        }
    }
}
