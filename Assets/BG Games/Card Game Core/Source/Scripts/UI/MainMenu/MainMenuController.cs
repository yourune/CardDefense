using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private RectTransform _screen;
        [Space] [SerializeField] private Button _startGame;
        [SerializeField] private Button _deckBuilding;
        [Space] [SerializeField] private Button _exit;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _profile;
        
        private SceneLoader _sceneLoader;
        private PreMatchDeckSelect _deckSelect;
        private SettingsController _settingsController; 
        private PlayerProfileController _playerProfileController;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, PreMatchDeckSelect deckSelect, 
            SettingsController settingsPanel, PlayerProfileController playerProfileController)
        {
            _sceneLoader = sceneLoader;
            _deckSelect = deckSelect;
            _settingsController = settingsPanel;
            _playerProfileController = playerProfileController;
        }

        private void Start()
        {
            _startGame.onClick.AddListener(StartGame);
            _deckBuilding.onClick.AddListener(ToDeckBuilding);

            _exit.onClick.AddListener(CloseApp);
            _settings.onClick.AddListener(Settings);
            _profile.onClick.AddListener(OpenProfile);
        }

        public void OpenProfile()
        {
            _playerProfileController.Open();
        }
        
        public void Open()
        {
            _screen.gameObject.SetActive(true);
        }

        public void Close()
        {
            _screen.gameObject.SetActive(false);
        }

        private void StartGame()
        {
            _deckSelect.Open();
            Close();
        }

        private void ToDeckBuilding()
        {
            _sceneLoader.LoadDeckBuilding();
        }

        public void DeckSelected()
        {
            _sceneLoader.LoadGame();
        }

        private void Settings()
        {
            _settingsController.Open();
        }

        private void CloseApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}