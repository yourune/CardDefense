using BG_Games.Card_Game_Core.Systems.Audio;
using BG_Games.Card_Game_Core.Systems.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.MainMenu
{
    class SettingsController:MonoBehaviour
    {
        [SerializeField] protected RectTransform Screen;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Dropdown _languageDropdown;
        
        private AudioSystem _audioSystem;
        private LocalizationSystem _localizationSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem, LocalizationSystem localizationSystem)
        {
            _audioSystem = audioSystem;
            _localizationSystem = localizationSystem;
        }

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
            _volumeSlider.onValueChanged.AddListener(VolumeChange);
        }

        private void Start()
        {
            _volumeSlider.value = _audioSystem.MasterSoundVolume;
            _volumeSlider.onValueChanged.Invoke(_volumeSlider.value);
            
            _localizationSystem.PopulateDropdown(_languageDropdown);
        }

        public void Close()
        {
            Screen.gameObject.SetActive(false);
        }

        public void Open()
        {
            Screen.gameObject.SetActive(true);
        }

        private void VolumeChange(float value)
        {
            _audioSystem.SetMasterVolume(value);
        }
    }
}
