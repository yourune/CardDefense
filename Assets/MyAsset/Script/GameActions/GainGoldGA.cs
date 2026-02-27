public class GainGoldGA : GameAction
{
    public int Amount { get; private set; }
    
    public GainGoldGA(int amount)
    {
        Amount = amount;
    }
}
