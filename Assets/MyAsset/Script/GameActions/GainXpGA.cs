public class GainXpGA : GameAction
{
    public int Amount { get; private set; }
    
    public GainXpGA(int amount)
    {
        Amount = amount;
    }
}
