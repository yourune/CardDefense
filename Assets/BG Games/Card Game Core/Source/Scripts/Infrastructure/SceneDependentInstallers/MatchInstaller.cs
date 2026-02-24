using BG_Games.Card_Game_Core.Cards.Factories;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.SceneDependentInstallers
{
    public class MatchInstaller : MonoInstaller
    {        
        [SerializeField] private PlayerRegistry _playerRegistry;
        [SerializeField] private MatchEnd _matchEnd;
        [SerializeField] private GameTable _gameTable;
        [SerializeField] private TurnSwitch _turnSwitch;
        [SerializeField] private MatchSettings _matchSettings;
        [SerializeField] private BattleLog _battleLog;
        [Space] 
        [SerializeField] private LayerMask _attackableLayerMask;        
        [SerializeField] private EventSystem _eventSystem;
        [Space] 
        [SerializeField] private CardFactoriesSettings _cardFactoriesSettings;

        public override void InstallBindings()
        {
            InstallCardFactories();

            InstallMatchSettings();
            InstallPlayers();
            InstallTurnSwitch();
            InstallInputLockSystem();
            InstallMatchEnd();
            InstallBattleLog();

            InstallLayerMasks();

            InstallTable();
        }

        private void InstallBattleLog()
        {
            Container.Bind<BattleLog>().FromInstance(_battleLog).AsSingle();
        }

        private void InstallMatchEnd()
        {
            Container.Bind<MatchEnd>().FromInstance(_matchEnd).AsSingle();
        }

        private void InstallMatchSettings()
        {
            Container.Bind<MatchSettings>().FromInstance(_matchSettings).AsSingle();
        }

        private void InstallInputLockSystem()
        {
            Container.Bind<EventSystem>().FromInstance(_eventSystem).AsSingle();
            Container.Bind<InputLockSystem>().FromNew().AsSingle();
        }

        private void InstallTurnSwitch()
        {
            Container.Bind<TurnSwitch>().FromInstance(_turnSwitch).AsSingle();
        }

        private void InstallPlayers()
        {
            Container.Bind<PlayerRegistry>().FromInstance(_playerRegistry).AsSingle();
        }

        private void InstallLayerMasks()
        {
            Container.Bind<LayerMask>().WithId("Attackable").FromInstance(_attackableLayerMask).AsSingle();
        }

        private void InstallTable()
        {
            Container.Bind<GameTable>().FromInstance(_gameTable).AsSingle();
        }

        private void InstallCardFactories()
        {
            Container.Bind<CardFactoriesSettings>().FromInstance(_cardFactoriesSettings).AsSingle();
        }
    }
}