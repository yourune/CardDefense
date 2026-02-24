namespace BG_Games.Card_Game_Core.Systems.CurrencySystem
{
    public interface ICurrencyService
    {
        int GoldCoins { get; }
        void AddGoldCoins(int coins);
        bool TryPayGoldCoins(int coins);
    }
}
