namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class NameUpdatedEvent
    {
        public string Name { get; }
        
        public NameUpdatedEvent(string name)
        {
            Name = name;
        }
    }
}