using BG_Games.Card_Game_Core.Tools;
using UnityEngine;
using UnityEngine.Localization;

namespace BG_Games.Card_Game_Core.Cards.Info
{
    public enum CardRace
    {
        Human,
        Allien,
        Neutral
    }

    public enum CardRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public abstract class CardInfo : ScriptableObject
    {
        [field: ScriptableObjectId]
        [field: SerializeField]
        public string Id { get; private set; }

        [field: SerializeField] public LocalizedString Name { get; private set; }    
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public CardRace Race { get; private set; }
        [field: SerializeField] public CardRarity Rarity { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
    }
}
