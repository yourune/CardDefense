using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    class AllAllyUnitsProvider:ITargetProvider
    {
        public event Action<bool> OnAimEnd;
        public List<UnitCard> Targets { get; private set; }

        private TableSide _myTableSide;

        public AllAllyUnitsProvider(PlayerId owner, GameTable table)
        {
            _myTableSide = table.GetMyTableSide(owner);
        }

        public void StartAim()
        {
            Targets = new List<UnitCard>();

            foreach (ITroopCard card in _myTableSide.TroopCards)
            {
                if (card is UnitCard unitCard)
                {
                    Targets.Add(unitCard);
                }
            }

            OnAimEnd?.Invoke(true);
        }
    }
}
