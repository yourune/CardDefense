using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    class SingleEnemyUnitProvider:ConcreteTargetProvider
    {
        private UnitCard _target;

        public UnitCard Target => _target;

        protected TableSide _enemyTableSide;

        public SingleEnemyUnitProvider(PlayerId owner, IAimSystem aimSystem, GameTable table, [Inject(Id = "Attackable")] LayerMask cardsLayerMask) : base(owner, aimSystem, table, cardsLayerMask)
        {
            _enemyTableSide = table.GetOpponentTableSide(owner);
        }

        public override TargetType TargetType => TargetType.Enemy;

        protected override bool ValidatePoint(Vector3 point)
        {
            return _enemyTableSide.IsInsideTableSide2D(point);
        }

        protected override bool TrySetTarget(Collider2D raycastResult)
        {
            return raycastResult.TryGetComponent<UnitCard>(out _target) && !_target.Logic.ContainsDescriptorFromList(ForbiddenDescriptors);
        }
    }
}
