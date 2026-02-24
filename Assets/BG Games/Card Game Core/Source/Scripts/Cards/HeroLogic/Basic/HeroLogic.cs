using System;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.HeroLogic.Basic
{
    public abstract class HeroLogic:TroopLogic,IHeroLogic
    {
        [Inject]
        protected BattleLog BattleLog;
        [Inject]
        protected PlayerId Owner;

        public abstract event Action OnApply;

        public int AbillityCost { get; protected set; }
        public virtual bool AbillityAvailable { get; protected set; }
        public HeroCardInfo HeroInfo { get; protected set; }

        public HeroLogic(HeroCardInfo info) : base(info)
        {
            AbillityCost = info.CardInfo.Cost;
            HeroInfo = info;
        }
        public override void NextTurn()
        {
            AbillityAvailable = true;
            base.NextTurn();
        }

        public virtual void ApplyAbillity(ITargetProvider targetProvider)
        {
            AbillityAction(targetProvider);
            AbillityAvailable = false;

            LogAbillityUse();
        }

        public void Draw()
        {
            // RemoveHP(HP);
        }

        protected abstract void AbillityAction(ITargetProvider targetProvider);

        protected virtual void LogAbillityUse()
        {
            BattleLog.LogHeroAbility(HeroInfo, Owner);
        }
    }
}
