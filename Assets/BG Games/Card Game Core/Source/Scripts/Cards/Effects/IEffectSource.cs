using System;

namespace BG_Games.Card_Game_Core.Cards.Effects
{
    public interface IEffectSource
    {
        public event Action OnRemoveEffect;
    }
}
