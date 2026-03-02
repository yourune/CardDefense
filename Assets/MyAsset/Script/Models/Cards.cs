using System.Collections.Generic;
using UnityEngine;

public class Cards
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public Sprite CastImage => data.CastImage;
    public Effect ManualTargetEffect => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    public List<AutoAreaEffect> AreaEffects => data.AreaEffects;
    public float AoeRadius => data.AoeRadius;
    public int Mana { get; private set; }

    private readonly CardData data;

    public Cards(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
    }
}
