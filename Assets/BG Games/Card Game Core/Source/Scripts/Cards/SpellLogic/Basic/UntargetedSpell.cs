using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic.Basic
{
    public abstract class UntargetedSpell:ISpellLogic
    {
        public SpellCardInfo SpellInfo { get; protected set; }
        public CardInfo Info { get; protected set; }

        public event Action OnApply;  

        [Inject]
        protected BattleLog BattleLog;
        [Inject]
        protected PlayerId Owner;

        protected UntargetedSpell(SpellCardInfo info)
        {
            Info = info;
            SpellInfo = info;
        }

        public virtual void CastSpell(ITargetProvider targetProvider)
        {
            SpellAction(targetProvider);

            OnApply?.Invoke();
            LogCast();
        }

        protected abstract void SpellAction(ITargetProvider targetProvider);

        protected virtual void LogCast()
        {
            BattleLog.LogPlayCard(Info, Owner);
        }
    }
}
