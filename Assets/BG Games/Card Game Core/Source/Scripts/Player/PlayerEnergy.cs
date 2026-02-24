using System;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        [SerializeField] private int _startEnergy = 1;
        [SerializeField] private int _energyGain = 1;
        [SerializeField] private int _maxBalanace = 10;
        [SerializeField]private int _balance;

        public event Action<int> OnEnergyBalanceChanged; 

        private int _currentTurnEnergy;

        public int TurnEnergy
        {
            get => _currentTurnEnergy;
            set => _currentTurnEnergy = Mathf.Clamp(value, 0, _maxBalanace);
        }

        public int Balance
        {
            get => _balance;
            private set
            {
                _balance = value;
                OnEnergyBalanceChanged?.Invoke(_balance);
            }
        }

        public void InitEnergy()
        {
            TurnEnergy = _startEnergy;
        }

        public void GainEnergy()
        {
            TurnEnergy += _energyGain;
            Balance = TurnEnergy;
        }

        public bool SpendEnergy(int amount)
        {
            if (CanSpend(amount))
            {
                Balance -= amount;
                
                return true;
            }
            else
                return false;
        }

        public bool CanSpend(int amount)
        {
            return amount <= Balance;
        }

        public void AddExtraEnergy(int amount)
        {
            Balance += amount;
        }
    }
}