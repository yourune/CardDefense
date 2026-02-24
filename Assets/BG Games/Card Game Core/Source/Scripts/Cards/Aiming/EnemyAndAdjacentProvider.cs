using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    class EnemyAndAdjacentProvider:ConcreteTargetProvider
    {
        public List<ITroopCard> AdjacentTargets { get; private set; }
        public ITroopCard PrimaryTarget { get; private set; }

        protected TableSide _enemyTableSide;
        public override TargetType TargetType => TargetType.Enemy;

        public EnemyAndAdjacentProvider(PlayerId owner, IAimSystem aimSystem, GameTable table, [Inject(Id = "Attackable")] LayerMask cardsLayerMask) : base(owner, aimSystem, table, cardsLayerMask)
        {
            _enemyTableSide = table.GetOpponentTableSide(owner);
        }

        protected override bool ValidatePoint(Vector3 point)
        {
            return _enemyTableSide.IsInsideTableSide2D(point);
        }

        protected override bool TrySetTarget(Collider2D raycastResult)
        {
            AdjacentTargets = new List<ITroopCard>();

            ITroopCard primaryTarget;

            if (raycastResult.TryGetComponent<ITroopCard>(out primaryTarget) && !primaryTarget.Logic.ContainsDescriptorFromList(ForbiddenDescriptors))
            {
                PrimaryTarget = primaryTarget;

                int primaryIndex = _enemyTableSide.TroopCards.IndexOf(primaryTarget);

                TryAddAdjacent(primaryIndex);
                return true;
            }
            else
                return false;
        }

        private void TryAddAdjacent(int primary)
        {
            if (primary == -1)
                return;

            if (primary > 0)
            {
                AdjacentTargets.Add(_enemyTableSide.TroopCards[primary - 1]);
            }

            if (primary < _enemyTableSide.Cards.Count - 1)
            {
                AdjacentTargets.Add(_enemyTableSide.TroopCards[primary + 1]);
            }

            string selectedCards = $"Selected cards: (primary = {primary}) \n" + PrimaryTarget.Logic.ToString() + "\n";
            foreach (ITroopCard target in AdjacentTargets)
            {
                selectedCards += target.Logic.ToString() + "\n";
            }

            Debug.Log(selectedCards);
        }

    }
}
