using UnityEngine;

public class CastleView : CombatantView
{
    public void Setup(CastleData castleData)
    {
        SetupBase(castleData.Health, castleData.Image);
    }
}
