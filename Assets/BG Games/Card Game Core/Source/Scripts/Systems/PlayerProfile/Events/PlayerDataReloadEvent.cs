namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class PlayerDataReloadEvent
    {
        public PlayerData Data;
        
        public PlayerDataReloadEvent(PlayerData data)
        {
            Data = data;
        }
    }
}
