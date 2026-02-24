using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using BG_Games.Card_Game_Core.UI;
using BG_Games.Card_Game_Core.UI.MainMenu;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Infrastructure.SceneDependentInstallers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuController _menuController;
        [SerializeField] private PreMatchDeckSelect _deckSelect;
        [SerializeField] private SettingsController _settingsController;
        [SerializeField] private PlayerProfileController _playerProfileController;
        [SerializeField] private Postman _postman;
        [Space]
        [SerializeField] private DeckSelectorFactory _deckSelectorFactory;

        public override void InstallBindings()
        {
            InstallPostman();
            InstallMenu();
            InstallDeckSelect();
            InstallSettings();
            InstallProfile();

            InstallDeckSelectorFactory();
        }

        private void InstallSettings()
        {
            Container.Bind<SettingsController>().FromInstance(_settingsController).AsSingle();
        }

        private void InstallProfile()
        {
            Container.Bind<PlayerProfileController>().FromInstance(_playerProfileController).AsSingle();
        }

        private void InstallDeckSelect()
        {
            Container.Bind<PreMatchDeckSelect>().FromInstance(_deckSelect).AsSingle();
        }

        private void InstallMenu()
        {
            Container.Bind<MainMenuController>().FromInstance(_menuController).AsSingle();
        }

        private void InstallDeckSelectorFactory()
        {
            Container.Bind<DeckSelectorFactory>().FromInstance(_deckSelectorFactory).AsSingle();
        }

        private void InstallPostman()
        {
            Container.Bind<Postman>().FromInstance(_postman).AsSingle();
        }
    }
}