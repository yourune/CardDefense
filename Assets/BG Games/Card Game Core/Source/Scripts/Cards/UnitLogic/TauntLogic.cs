using System;
using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Effects;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.UnitLogic.Basic;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.Cards.UnitLogic
{
    public class TauntLogic:UnitCardLogic,IEffectSource
    {
        public event Action OnRemoveEffect;

        private TableSide _playerTableSide;
        private PlayerHero _playerHero;
        
        public TauntLogic(UnitCardInfo info, GameTable table, PlayerId owner, PlayerHero hero):base(info)
        {
            _playerTableSide = table.GetMyTableSide(owner);
            _playerHero = hero;
        }

        public override void SubscribeCardControllerCallbacks(UnitCard controller)
        {
            controller.OnPlaced += ApplyAbillity;
        }

        private void ApplyAbillity()
        {
            TauntEffect effect = new TauntEffect(_playerTableSide, _playerHero);
            effect.Apply(this, this);
        }

        protected override void Dead()
        {
            OnRemoveEffect?.Invoke();
            base.Dead();
        }
    }
}
