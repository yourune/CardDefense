using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.Effects;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Cards
{
    public abstract class TroopLogic:ITroopLogic
    {
        public event Action<int> OnHPChanged;
        public event Action<int> OnAttacked;
        public event Action OnDead;
        public event Action<int> OnRemoveHP;
        public event Action<int> OnHeal;
        public List<IDescriptor> Descriptors { get; protected set; } = new List<IDescriptor>();
        protected List<ITemporaryEffect> Effects = new List<ITemporaryEffect>();

        public CardInfo Info { get; protected set; }

        protected int hp;

        public int DefaultHP { get; protected set; }

        public int HP
        {
            get => hp;
            protected set
            {
                hp = value;
                OnHPChanged?.Invoke(hp);

                if (hp <= 0)
                {
                    Dead();
                }
            }
        }

        public TroopLogic(ICardTroopInfo info)
        {
            Info = info.CardInfo;
            hp = info.HP;
            DefaultHP = info.HP;
        }

        public virtual void NextTurn()
        {
            ITemporaryEffect[] effects = new ITemporaryEffect[Effects.Count];
            Effects.CopyTo(effects);

            foreach (var effect in effects)
            {
                effect.NextTurn();
            }
        }

        public virtual void EndTurn()
        {
            ITemporaryEffect[] effects = new ITemporaryEffect[Effects.Count];
            Effects.CopyTo(effects);

            foreach (var effect in effects)
            {
                effect.EndTurn();
            }
        }

        public virtual void GainHP(int amount)
        {
            HP += amount;
            OnHeal?.Invoke(amount);
        }

        public virtual void RestoreHP(int amount)
        {
            int newValue = Mathf.Clamp(hp + amount, 0, DefaultHP);
            int delta = newValue - HP;

            HP = newValue;

            OnHeal?.Invoke(delta);
        }

        public virtual void RemoveHP(int amount)
        {
            HP -= amount;
            OnRemoveHP?.Invoke(amount);
        }

        public virtual void Attacked(int amount, ITroopLogic attacker)
        {
            Attacked(amount);
        }

        public virtual void Attacked(int amount)
        {
            HP -= amount;            
            OnAttacked?.Invoke(amount);
        }

        public virtual void AddTemporaryEffect(ITemporaryEffect effect)
        {
            Effects.Add(effect);
        }

        public virtual void RemoveTemporaryEffect(ITemporaryEffect effect)
        {
            Effects.Remove(effect);
        }

        protected virtual void Dead()
        {
            OnDead?.Invoke();

            ClearEffects();
        }

        public void ClearEffects()
        {
            ITemporaryEffect[] effects = new ITemporaryEffect[Effects.Count];
            Effects.CopyTo(effects);

            foreach (var effect in effects)
            {
                effect.Remove();
            }
        }
    }
}
