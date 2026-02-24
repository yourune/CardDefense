using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Cards.Descriptors;
using BG_Games.Card_Game_Core.Cards.UnitLogic;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.Cards.Effects
{
    class TauntEffect:TemporaryEffect
    {
        private TableSide _playerTableSide;
        private PlayerHero _playerHero;

        public TauntEffect(TableSide tableSide, PlayerHero hero)
        {
            _playerTableSide = tableSide;
            _playerHero = hero;
        }

        protected override void ApplyEffect()
        {
            _playerTableSide.OnPlacedCard += PlacedNewCard;

            _playerHero.Card.Logic.Descriptors.Add(new TauntedDescriptor());

            foreach (ITroopCard card in _playerTableSide.TroopCards)
            {
                if (card.Logic != Target)
                {
                    card.Logic.Descriptors.Add(new TauntedDescriptor());
                }
            }
        }

        private void PlacedNewCard(Card card)
        {
            ITroopCard cardTroop = card as ITroopCard;
            if (cardTroop == null)
            {
                return;
            }

            if (!(cardTroop.Logic is TauntLogic))
            {
                cardTroop.Logic.Descriptors.Add(new TauntedDescriptor());
            }
        }

        public override void Remove()
        {
            _playerTableSide.OnPlacedCard -= PlacedNewCard;

            foreach (ITroopCard card in _playerTableSide.TroopCards)
            {
                card.Logic.RemoveDescriptor<TauntedDescriptor>();
            }

            _playerHero.Card.Logic.RemoveDescriptor<TauntedDescriptor>();            
            
            base.Remove();
        }
    }
}
