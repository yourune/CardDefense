using System;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [Serializable]
    public class PlayerData
    {
        public string AvatarImageId;
        public string Name;
        
        public int Level;
        public int AllTimePoints;
        public int GoldCoins;
        
        public PlayerData()
        {
            AvatarImageId = string.Empty;
            Name = PlayerProfileConsts.DefaultProfileName;
            
            Level = PlayerProfileConsts.DefaultPlayerLevel;
            AllTimePoints = 0;
            GoldCoins = 0;
        }
    }
}