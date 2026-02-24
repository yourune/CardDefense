using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.GenericInstallers
{
    class SettingsInstaller:MonoInstaller
    {
        [SerializeField] private RaritiesSettings _raritiesSettings;
        [SerializeField] private CardViewSettings _viewSettings;
        [SerializeField] private PlayerInventorySettings _playerInventorySettings;

        public override void InstallBindings()
        {
            InstallRaritySettings();
        }

        private void InstallRaritySettings()
        {
            Container.Bind<RaritiesSettings>().FromInstance(_raritiesSettings).AsSingle();
            Container.Bind<CardViewSettings>().FromInstance(_viewSettings).AsSingle();
            Container.Bind<PlayerInventorySettings>().FromInstance(_playerInventorySettings).AsSingle();
        }
    }
}
