using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic.Basic
{
    public interface IHeroLogic:ITroopLogic
    {
        public event Action OnApply;

        public int AbillityCost { get; }
        public bool AbillityAvailable { get; }
        public HeroCardInfo HeroInfo { get; }

        public void ApplyAbillity(ITargetProvider targetProvider);
        public void Draw();
    }
}
