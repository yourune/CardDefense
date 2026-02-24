using BG_Games.Card_Game_Core.Cards.Aiming;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Cards.SpellLogic.Basic;
using BG_Games.Card_Game_Core.Player;

namespace BG_Games.Card_Game_Core.Cards.SpellLogic
{
    class DrawCards:UntargetedSpell
    {
        private int _drawCardsCount;
        private DrawCard _drawCard;

        public DrawCards(SpellCardInfo info, DrawCard drawCard, int drawCardsCount):base(info)
        {
            _drawCard = drawCard;
            _drawCardsCount = drawCardsCount;
        }

        protected override async void SpellAction(ITargetProvider targetProvider)
        {
            for (int i = 0; i < _drawCardsCount; i++)
            {
                await _drawCard.DrawNextCard();
            }
        }

    }
}
