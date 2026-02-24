using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Aiming
{
    public class SingleAllyUnitProvider:ConcreteTargetProvider
    {
        private UnitCard _target;
        public UnitCard Target { get => _target; }

        protected TableSide _myTableSide;

        public SingleAllyUnitProvider(PlayerId owner, IAimSystem aimSystem, GameTable table, [Inject(Id = "Attackable")] LayerMask cardsLayerMask) : base(owner, aimSystem, table, cardsLayerMask)
        {
            _myTableSide = table.GetMyTableSide(owner);
        }

        public override TargetType TargetType => TargetType.Ally;

        protected override bool ValidatePoint(Vector3 point)
        {
            return _myTableSide.IsInsideTableSide2D(point);
        }

        protected override bool TrySetTarget(Collider2D raycastResult)
        {
            return raycastResult.TryGetComponent<UnitCard>(out _target) && !_target.Logic.ContainsDescriptorFromList(ForbiddenDescriptors);
        }
    }
}
