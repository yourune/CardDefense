using BG_Games.Card_Game_Core.Cards.Controllers;
using BG_Games.Card_Game_Core.Player.Mulligan;
using BG_Games.Card_Game_Core.Systems;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Player
{
    public class DrawCard:MonoBehaviour
    {
        private int _startCardsSetCount;

        private PlayerHand _playerHand;
        private MulliganHandler _mulliganHandler;
        private PlayerDeck _playerDeck;
        private PlayerHero _playerHero;

        [Inject]
        private void Construct(PlayerHand playerHand,PlayerDeck playerDeck,MulliganHandler mulliganHandler,PlayerHero hero,MatchSettings settings,Player player)
        {
            _playerHand = playerHand;
            _playerDeck = playerDeck;
            _mulliganHandler = mulliganHandler;
            _playerHero = hero;
            _startCardsSetCount = settings.InitialHandSize;
        }

        public async UniTask<Card> DrawNextCard()
        {
            if (_playerDeck.LeftCards)
            {
                Card card = await _playerDeck.DrawCard();
                _playerHand.AddCard(card);
                return card;
            }
            else return null;
        }

        public async UniTask<Card[]> DrawCards(int amount)
        {
            return await _playerDeck.DrawCards(amount);
        }

        public async UniTask InitStartHand()
        {
            Card[] startHand = await _playerDeck.DrawCards(_startCardsSetCount);

            foreach (Card card in startHand)
            {
                _mulliganHandler.AddCard(card);
            }
        }
    }
}
