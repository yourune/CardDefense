using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    class SeveralRandomAllyProvider:ITargetProvider
    {
        public event Action<bool> OnAimEnd;

        public List<ITroopCard> Targets { get; private set; }

        private int _amountOfTargets;
        private TableSide _myTableSide;

        public SeveralRandomAllyProvider(int amountOfTargets,PlayerId owner, GameTable table)
        {
            _amountOfTargets = amountOfTargets;
            _myTableSide = table.GetMyTableSide(owner);
        }

        public void StartAim()
        {
            Targets = new List<ITroopCard>();

            if (_amountOfTargets >= _myTableSide.Cards.Count)
            {
                SetAllAsTargets();
            }
            else
            {
                SetRandomNonRepetitiveAsTargets();
            }
            
            OnAimEnd?.Invoke(true);
        }

        private void SetAllAsTargets()
        {
            foreach (var card in _myTableSide.TroopCards)
            {
                Targets.Add(card);
            }
        }

        private void SetRandomNonRepetitiveAsTargets()
        {
            int[] randomIndexes = new int[_amountOfTargets];

            for (int i = 0; i < _amountOfTargets; i++)
            {
                int randomNonRepetitive;

                do
                {
                    randomNonRepetitive = UnityEngine.Random.Range(0, _myTableSide.TroopCards.Count);
                } while (randomIndexes.Contains(randomNonRepetitive));

                randomIndexes[i] = randomNonRepetitive;
            }

            foreach (int i in randomIndexes)
            {
                Targets.Add(_myTableSide.TroopCards[i]);
            }
        }
    }
}
