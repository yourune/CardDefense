using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;

namespace BG_Games.Card_Game_Core.Systems.CurrencySystem
{
    public class CurrencyService : ICurrencyService
    {
        public int GoldCoins => _playerData.GoldCoins;
        
        private PlayerData _playerData;
        
        public CurrencyService()
        {
            _playerData = SaveService.ProfileDataHandler.LoadProfileData();
            EventBus.Subscribe<PlayerDataReloadEvent>(UpdatePlayerData);
        }

        public void AddGoldCoins(int coins)
        {
            _playerData.GoldCoins += coins;
            SaveService.ProfileDataHandler.SaveProfileData();
            EventBus.Publish(new GoldCoinsUpdateEvent(_playerData.GoldCoins, coins));
        }

        public bool TryPayGoldCoins(int coins)
        {
            if (GoldCoins < coins) 
                return false;
            
            AddGoldCoins(-coins);
            return true;
        }

        private void UpdatePlayerData(PlayerDataReloadEvent data)
        {
            _playerData = data.Data;
            EventBus.Publish(new GoldCoinsUpdateEvent(_playerData.GoldCoins, 0));
        }
    }
}
