namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class AvatarSelectedEvent
    {
        public ProfileAvatarConfig ProfileAvatarConfig { get; }
        
        public AvatarSelectedEvent(ProfileAvatarConfig profileAvatarConfig)
        {
            ProfileAvatarConfig = profileAvatarConfig;
        }
    }
}