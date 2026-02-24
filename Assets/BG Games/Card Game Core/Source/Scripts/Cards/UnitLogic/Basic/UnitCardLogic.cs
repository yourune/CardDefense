using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.Effects;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic.Basic
{
    public abstract class UnitCardLogic : TroopLogic,IUnitLogic
    {
        public event Action<int> OnDPChanged;
        public event Action<Vector3> OnAttack;
        public event Action<int> OnCounterAttacked;

        protected UnitCardInfo UnitInfo;
        private int _dp;

        public bool ActionAvailable { get; protected set; } = false;
        public int DP
        {
            get => _dp;
            set
            {
                _dp = value;
                OnDPChanged?.Invoke(_dp);
            }
        }

        [Inject]
        protected BattleLog BattleLog;
        [Inject]
        protected PlayerId Owner;

        public UnitCardLogic(UnitCardInfo info):base(info)
        {
            UnitInfo = info;
            _dp = info.DP;
        }

        public virtual void SubscribeCardControllerCallbacks(UnitCard controller){}

        public void ResetCard()
        {
            ActionAvailable = false;

            Effects = new List<ITemporaryEffect>();
            Descriptors = new List<IDescriptor>();

            HP = UnitInfo.HP;
            DP = UnitInfo.DP;
        }

        public override void NextTurn()
        {
            ActionAvailable = true;

            base.NextTurn();
        }

        public virtual void DoMainAction(ITargetProvider targetProvider)
        {
            if (ActionAvailable)
            {
                SingleEnemyProvider provider = targetProvider as SingleEnemyProvider;

                if (provider != null)
                {                    
                    ActionAvailable = false;
                    AttackCard(provider.Target);

                    BattleLog.LogAttack(UnitInfo,provider.Target.Logic.Info,Owner);
                }
                else
                {
                    throw new Exception($"Unit should have {nameof(SingleEnemyProvider)}");
                }
            }
        }

        public virtual void GainDP(int amount)
        {
            DP += amount;
        }

        public virtual void RemoveDP(int amount)
        {
            DP -= amount;
        }

        public override void Attacked(int amount, ITroopLogic attacker)
        {
            Attacked(amount);
            if (attacker is IUnitLogic)
            {
                (attacker as IUnitLogic).Counterattacked(DP,this);
            }
        }

        public virtual void Counterattacked(int amount, IUnitLogic enemy)
        {
            HP -= amount;
            OnCounterAttacked?.Invoke(amount);
        }

        public virtual bool WillCounterattack(ITroopLogic attacker) => true;
        public virtual bool WillBeKilled(int amount, ITroopLogic attacker) => HP <= amount;
        public virtual int CalculateCounterattackDamage(ITroopLogic attacker) => DP;

        protected virtual void AttackCard(ITroopCard target)
        {
            target.Logic.Attacked(DP, this);
            OnAttack?.Invoke(target.Position);
        }
    }
}
