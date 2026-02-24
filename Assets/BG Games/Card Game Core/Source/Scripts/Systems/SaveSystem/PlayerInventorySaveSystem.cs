using BG_Games.Card_Game_Core.Systems.PlayerProfile;

namespace BG_Games.Card_Game_Core.Systems.SaveSystem
{
    public class PlayerInventorySaveSystem : SaveDataSystem<InventoryData>, IPlayerInventorySaveSystem
    {
        private readonly PlayerInventorySettings _playerInventorySettings;
        
        protected override string Key => PlayerProfileConsts.InventoryDataKey;

        public PlayerInventorySaveSystem(PlayerInventorySettings playerInventorySettings)
        {
            _playerInventorySettings = playerInventorySettings;
        }

        public override InventoryData Load()
        {
            InventoryData data = base.Load();

            if (data.Cards.Count == 0)
            {
                foreach (var card in _playerInventorySettings.DefaultCards)
                {
                    data.Cards.Add(card.Id);
                }
            }

            return data;
        }

        public bool ContainsCardId(string cardId)
        {
            return Load().Cards.Contains(cardId);
        }

        public void AddCardId(string cardId)
        {
            var data = Load();
            if (!data.Cards.Contains(cardId))
            {
                data.Cards.Add(cardId);
                Save();
            }
        }
    }
}
