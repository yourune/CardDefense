using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic.Basic
{
    public abstract class TargetedSpell:ISpellLogic
    {
        public event Action<Vector3> OnTargetedApply;
        public event Action OnApply;

        public SpellCardInfo SpellInfo { get; protected set; }
        public CardInfo Info { get; protected set; }

        [Inject]
        protected BattleLog BattleLog;
        [Inject]
        protected PlayerId Owner;

        protected TargetedSpell(SpellCardInfo info)
        {
            Info = info;
            SpellInfo = info;
        }

        public virtual void CastSpell(ITargetProvider targetProvider)
        {
            ICard target = SpellAction(targetProvider);         
            
            OnTargetedApply?.Invoke(target.Position);
            OnApply?.Invoke();

            LogCast(target.Info);
        }

        protected abstract ICard SpellAction(ITargetProvider targetProvider);

        protected virtual void LogCast(CardInfo target)
        {
            BattleLog.LogPlaySpell(Info, target, Owner);
        }
    }
}
