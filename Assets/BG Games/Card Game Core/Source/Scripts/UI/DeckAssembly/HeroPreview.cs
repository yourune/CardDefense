using System;
using System.Linq;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{
    class HeroPreview : MonoBehaviour, IUpdatesLocalization
    {
        [SerializeField] private HeroPreviewConfig _config;
        [Space] [SerializeField] private Button _chooseHero;
        [SerializeField] private RectTransform _screen;
        [SerializeField] private Image _frame;
        [SerializeField] private TMP_Text _raceText;
        [Space] [SerializeField] private TMP_Text _heroName;
        [SerializeField] private TMP_Text _heroAllias;
        [SerializeField] private Image _heroImage;
        [SerializeField] private TMP_Text _abillityName;
        [SerializeField] private TMP_Text _abillityDescription;

        public event Action OnChooseClick;

        private HeroCardInfo _heroCardInfo;
        
        private void Awake()
        {
            _chooseHero?.onClick.AddListener(() => OnChooseClick?.Invoke());
            LocalizationSettings.SelectedLocaleChanged += OnLocaleUpdated;
        }
        
        public void SelectHero(HeroCardInfo info)
        {
            _heroCardInfo = info;
            _screen.gameObject.SetActive(true);
            HeroPreviewConfig.Record heroPreview = _config.Records.First(record => record.Race == info.Race);

            _frame.sprite = heroPreview.Frame;
            _heroImage.sprite = info.Image;

            SetLocalizedText();
        }

        public void OnLocaleUpdated(Locale locale)
        {
            SetLocalizedText();
        }

        public void SetLocalizedText()
        {
            _raceText.text = LocalizationData
                .GetLocalized(LocalizationData.KeyPrefixes.RaceLabel + _heroCardInfo.Race, LocalizationData.CardLocalizationTable);
            
            _heroName.text = _heroCardInfo.Name.GetLocalizedString();
            _abillityDescription.text = _heroCardInfo.Description.GetLocalizedString();
            _heroAllias.text = _heroCardInfo.Allias.GetLocalizedString();
            _abillityName.text = _heroCardInfo.AbillityName.GetLocalizedString();
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleUpdated;
        }
    }
}