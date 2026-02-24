using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics
{
    public class ProfileStatisticsView : MonoBehaviour
    {
        [SerializeField] private ProfileStatItem _profieStatItem;
        [SerializeField] private RectTransform _content;

        private void Start()
        {
            EventBus.Subscribe<DeleteProfileEvent>(OnDeleteProfile);
            LocalizationSettings.SelectedLocaleChanged += RespawnStats;
        }

        public void Init()
        {
            LoadStats();
        }

        public void LoadStats()
        {
            MatchData matchData = SaveService.MatchDataHandler.LoadMatchData();

            if (matchData == null)
            {
                return;
            }

            foreach (var stat in matchData.StatTypeValues)
            {
                ProfileStatItem statItem = Instantiate(_profieStatItem, _content);
                statItem.Init(
                    LocalizationData.GetLocalized(LocalizationData.KeyPrefixes.Label + stat.Key.ToString(),
                        LocalizationData.UILocalizationTable), stat.Value.ToString());
            }
        }

        private void RespawnStats(Locale locale)
        {
            RespawnStats();
        }

        private void RespawnStats()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }

            LoadStats();
        }

        private void OnDeleteProfile(DeleteProfileEvent obj)
        {
            RespawnStats();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<DeleteProfileEvent>(OnDeleteProfile);
            LocalizationSettings.SelectedLocaleChanged -= RespawnStats;
        }
    }
}