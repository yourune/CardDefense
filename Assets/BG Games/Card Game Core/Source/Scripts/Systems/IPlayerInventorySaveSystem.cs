using BG_Games.Card_Game_Core.Systems.PlayerProfile;

namespace BG_Games.Card_Game_Core.Systems
{
    public interface IPlayerInventorySaveSystem
    {
        InventoryData Load();
        void Save();
        void Delete();
        void AddCardId(string cardId);
        bool ContainsCardId(string cardId);
    }
}
