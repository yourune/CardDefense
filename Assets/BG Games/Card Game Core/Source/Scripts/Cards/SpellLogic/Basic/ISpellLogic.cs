using System;
using BG_Games.Card_Game_Core.Cards.Aiming;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic.Basic
{
    public interface ISpellLogic
    {
        public event Action OnApply;
        public void CastSpell(ITargetProvider targetProvider);
    }
}
