using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.Audio;
using BG_Games.Card_Game_Core.Systems.Localization;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.InGame
{
    class SettingsController:MonoBehaviour
    {
        [SerializeField] protected RectTransform Screen;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _drawButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Dropdown _languageDropdown;

        protected SceneLoader SceneLoader;
        protected Systems.MatchEnd MatchEnd;
        protected Player.Player Player;
        protected AudioSystem AudioSystem;
        protected LocalizationSystem LocalizationSystem;

        [Inject]
        protected virtual void Construct(SceneLoader sceneLoader, Systems.MatchEnd matchEnd, 
            PlayerRegistry playerRegistry, AudioSystem audioSystem, LocalizationSystem localizationSystem)
        {
            SceneLoader = sceneLoader;
            MatchEnd = matchEnd;
            Player = playerRegistry.LocalPlayer;
            AudioSystem = audioSystem;
            LocalizationSystem = localizationSystem;
        }

        private void Awake()
        {
            _exitButton.onClick.AddListener(Exit);
            _drawButton.onClick.AddListener(Draw);

            _openButton.onClick.AddListener(Open);
            _closeButton.onClick.AddListener(Close);

            _volumeSlider.onValueChanged.AddListener(VolumeChange);

            LocalizationSystem.PopulateDropdown(_languageDropdown);
        }

        private void Start()
        {
            _volumeSlider.value = AudioSystem.MasterSoundVolume;
            _volumeSlider.onValueChanged.Invoke(_volumeSlider.value);
        }

        private void Exit()
        {
            var data = SaveService.MatchDataHandler.LoadMatchData();
            data.AddStat(StatType.Losses, 1);
            SaveService.MatchDataHandler.SaveMatchData(data);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Draw()
        {
            Player.Hero.Draw();
            Close();
        }

        private void Close()
        {
            Screen.gameObject.SetActive(false);
        }

        private void Open()
        {
            Screen.gameObject.SetActive(true);
        }

        private void VolumeChange(float value)
        {
            AudioSystem.SetMasterVolume(value);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Screen.gameObject.activeSelf)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }
    }
}
