using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards.Info
{
    public class DeckCardInfo:CardInfo
    {
        [field: SerializeField] public HeroCardInfo Hero { get; private set; }
        [field: SerializeField] public int GoldPrice { get; private set; }
    }
}
