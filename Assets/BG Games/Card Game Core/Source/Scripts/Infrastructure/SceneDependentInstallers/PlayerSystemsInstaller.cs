using BG_Games.Card_Game_Core.Cards.Factories;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Player.Mulligan;
using BG_Games.Card_Game_Core.Player.SelectCard;
using BG_Games.Card_Game_Core.Player.TurnOrder;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.SceneDependentInstallers
{
    public class PlayerSystemsInstaller : MonoInstaller
    {
        [SerializeField] private Player.Player _player;
        [Space] 
        [SerializeField] private AimSystem _aimSystem;
        [SerializeField] private DeckProvider _deckProvider;
        [SerializeField] private PlayerHero _playerHero;
        [SerializeField] private DrawCard _drawCardSystem;
        [SerializeField] private PlayerHand _playerHand;
        [SerializeField] private PlayerEnergy _playerEnergy;
        [SerializeField] private PlayerDeck _playerDeck;
        [SerializeField] private MulliganHandler _mulliganHandler;
        [SerializeField] private SelectCardHandler _selectCardHandler;


        public override void InstallBindings()
        {
            InstallTurnOrderStates();
            InstallCardFactories();
            InstallDeckProvider();
            InstallAimSystem();

            InstallPlayer();

            InstallHero();
            InstallDrawCard();
            InstallHand();
            InstallEnergy();
            InstallDeck();
            InstallMulliganHandler();
            InstallSelectCard();
        }

        private void InstallSelectCard()
        {
            Container.Bind<SelectCardHandler>().FromInstance(_selectCardHandler).AsSingle();
        }

        protected virtual void InstallAimSystem()
        {
            Container.Bind<IAimSystem>().FromInstance(_aimSystem).AsSingle();
        }

        private void InstallDeckProvider()
        {
            Container.Bind<DeckProvider>().FromInstance(_deckProvider).AsSingle();
        }

        private void InstallCardFactories()
        {
            Container.Bind<CardFactory>().FromNew().AsSingle();
            Container.Bind<HeroCardFactory>().FromNew().AsSingle();
        }

        private void InstallMulliganHandler()
        {
            Container.Bind<MulliganHandler>().FromInstance(_mulliganHandler).AsSingle();
        }

        private void InstallHero()
        {
            Container.Bind<PlayerHero>().FromInstance(_playerHero).AsSingle();
        }

        private void InstallPlayer()
        {
            Container.Bind<Player.Player>().FromInstance(_player);
            Container.Bind<PlayerId>().FromInstance(_player.ID);
        }

        protected virtual void InstallTurnOrderStates()
        {
            Container.Bind<StartMatchTurnState>().FromNew().AsTransient();
            Container.Bind<FirstPreTurnState>().FromNew().AsTransient();
            Container.Bind<PreTurnState>().FromNew().AsTransient();
            Container.Bind<MainTurnState>().FromNew().AsTransient();
            Container.Bind<WaitingTurnState>().FromNew().AsTransient();
            Container.Bind<MulliganTurnState>().FromNew().AsTransient();
            Container.Bind<MatchEndTurnState>().FromNew().AsTransient();
        }

        private void InstallDeck()
        {
            Container.Bind<PlayerDeck>().FromInstance(_playerDeck).AsSingle();
        }

        private void InstallEnergy()
        {
            Container.Bind<PlayerEnergy>().FromInstance(_playerEnergy).AsSingle();
        }

        private void InstallHand()
        {
            Container.Bind<PlayerHand>().FromInstance(_playerHand).AsSingle();
        }

        private void InstallDrawCard()
        {
            Container.Bind<DrawCard>().FromInstance(_drawCardSystem).AsSingle();
        }
    }
}