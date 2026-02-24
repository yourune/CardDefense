using System;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic.Basic
{
    public interface ILogicWithSFX
    {
        public event Action OnSFXApply;
    }
}
