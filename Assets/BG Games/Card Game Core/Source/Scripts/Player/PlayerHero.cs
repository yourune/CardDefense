using System;
using BG_Games.Card_Game_Core.Cards.Controllers;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Player
{
    public class PlayerHero: MonoBehaviour
    {
        [SerializeField] private Transform _heroHolder;
        [SerializeField] private Transform _heroTooltipHolder;
        
        public Action OnHeroDead;
        public Action OnHeroDraw;

        private PlayerDeck _deck;

        public Vector3 CardPosition
        {
            get
            {
                if (Card != null)
                {
                    return Card.Position;
                }
                else
                {
                    return _heroHolder.position;
                }
            }
        }
        public Hero Card { get; private set; }

        [Inject]
        private void Construct(PlayerDeck deck)
        {
            _deck = deck;
        }

        public async void CreateHero()
        {
            Card = await _deck.GetHero();

            Card.InitPosition(_heroHolder);
            Card.Abillity.InitPosition(_heroTooltipHolder);

            Card.HeroLogic.OnDead += () => OnHeroDead?.Invoke();
        }
        
        public void Draw()
        {
            Card.HeroLogic.Draw();
            OnHeroDraw?.Invoke();
        }
    }
}
