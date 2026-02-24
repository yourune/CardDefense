using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Systems.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace BG_Games.Card_Game_Core.UI.CardInfoHolders
{
    class HeroAbillityInfoHolder:MonoBehaviour, IUpdatesLocalization
    {
        [SerializeField] private TMP_Text _abillityName;
        [SerializeField] private TMP_Text _abillityDescription;
        [SerializeField] private TMP_Text _cost;

        private HeroCardInfo _heroCardInfo;
        
        private void Awake()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleUpdated;
        }
        
        public void Init(HeroCardInfo info)
        {
            _heroCardInfo = info;
            
            _cost.text = info.Cost.ToString();
            SetLocalizedText();
        }

        public void SetLocalizedText()
        {
            _abillityName.text = _heroCardInfo.AbillityName.GetLocalizedString();
            _abillityDescription.text = _heroCardInfo.Description.GetLocalizedString();
        }

        public void OnLocaleUpdated(Locale locale)
        {
            SetLocalizedText();
        }
    }
}
