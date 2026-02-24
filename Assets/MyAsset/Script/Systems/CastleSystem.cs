using UnityEngine;

public class CastleSystem : Singleton<CastleSystem>
{
    [field: SerializeField] public CastleView CastleView { get; private set; }
    public void Setup(CastleData castleData)
    {
        CastleView.Setup(castleData);
    }
}
