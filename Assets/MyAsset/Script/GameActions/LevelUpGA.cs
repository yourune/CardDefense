public class LevelUpGA : GameAction
{
    public int NewLevel { get; private set; }
    
    public LevelUpGA(int newLevel)
    {
        NewLevel = newLevel;
    }
}
