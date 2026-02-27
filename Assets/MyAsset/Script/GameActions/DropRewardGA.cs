using UnityEngine;

public class DropRewardGA : GameAction
{
    public Vector3 DropPosition { get; private set; }
    public int GoldAmount { get; private set; }
    public int XpAmount { get; private set; }
    
    public DropRewardGA(Vector3 dropPosition, int goldAmount, int xpAmount)
    {
        DropPosition = dropPosition;
        GoldAmount = goldAmount;
        XpAmount = xpAmount;
    }
}
