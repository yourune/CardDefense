namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class GoldCoinsUpdateEvent
    {
        public int GoldCoins;
        public int AddGoldCoins;
        
        public GoldCoinsUpdateEvent(int coins, int addCoins)
        {
            GoldCoins = coins;
            AddGoldCoins = addCoins;
        }
    }
}
