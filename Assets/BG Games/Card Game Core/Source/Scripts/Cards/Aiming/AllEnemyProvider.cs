using System;
using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    class AllEnemyProvider:ITargetProvider
    {
        public event Action<bool> OnAimEnd;
        public List<ITroopCard> Targets { get; private set; }

        private TableSide _myTableSide;
        private Player.Player _owner;
        private Player.Player _enemy;

        public AllEnemyProvider(Player.Player owner, PlayerRegistry registry, GameTable table)
        {
            _owner = owner;
            _enemy = registry.GetOpponentOfPlayer(owner.ID);
            _myTableSide = table.GetOpponentTableSide(owner.ID);
        }

        public void StartAim()
        {
            Targets = new List<ITroopCard>();

            foreach (ITroopCard card in _myTableSide.TroopCards)
            {
                if (card is UnitCard)
                {
                    Targets.Add(card);
                }
            }
            Targets.Add(_enemy.Hero.Card);

            OnAimEnd?.Invoke(true);
        }
    }
}
