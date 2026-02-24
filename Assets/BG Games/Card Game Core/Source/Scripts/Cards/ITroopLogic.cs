using System;
using BG_Games.Card_Game_Core.Cards.Effects;
using BG_Games.Card_Game_Core.Cards.Info;

namespace BG_Games.Card_Game_Core.Cards
{
    public interface ITroopLogic:ILogicWithDescriptors
    {
        public event Action<int> OnHPChanged;
        public event Action<int> OnAttacked;
        public event Action<int> OnHeal;
        public event Action<int> OnRemoveHP;
        public event Action OnDead;

        public CardInfo Info { get; }
        public int HP { get; }
        public int DefaultHP { get; }

        public void NextTurn();
        public void EndTurn();

        public void GainHP(int amount);

        public void RestoreHP(int amount);

        public void RemoveHP(int amount);

        public void Attacked(int amount, ITroopLogic attacker);

        public void Attacked(int amount);

        public void AddTemporaryEffect(ITemporaryEffect effect);
        public void RemoveTemporaryEffect(ITemporaryEffect effect);
        public void ClearEffects();
    }
}
