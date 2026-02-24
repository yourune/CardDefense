using System.Linq;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.Systems.CurrencySystem;
using BG_Games.Card_Game_Core.Systems.PlayerProfile;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace BG_Games.Card_Game_Core.UI.InGame.MatchEnd
{
    class MatchEndController : MonoBehaviour
    {
        [SerializeField] protected RectTransform Screen;
        [SerializeField] private Button MenuButton;
        [SerializeField] protected Image BannerImage;
        [SerializeField] protected TMP_Text Text;
        [SerializeField] private MatchEndControllerBannerConfig _bannerConfig;
        [FormerlySerializedAs("_profileLevelConfigList")] [SerializeField] private PlayerLevelInfoList playerLevelInfoList;
        [SerializeField] private CurrencyConfig _currencyConfig;
        
        protected SceneLoader SceneLoader;
        protected Systems.MatchEnd MatchEnd;
        private ICurrencyService _currencyService;

        [Inject]
        protected virtual void Construct(SceneLoader sceneLoader, Systems.MatchEnd matchEnd, ICurrencyService currencyService)
        {
            SceneLoader = sceneLoader;
            MatchEnd = matchEnd;
            _currencyService = currencyService;
            
            matchEnd.OnWin += OnWin;
            matchEnd.OnDefeat += OnDefeat;
            matchEnd.OnDraw += OnDraw;
        }

        private void OnWin()
        {
            Open();
            var bannerConfig = _bannerConfig.Banners.FirstOrDefault(b => b.Type == MatchEndControllerBannerType.Win);
            Text.text = bannerConfig.Text.GetLocalizedString();
            BannerImage.sprite = bannerConfig.Banner;

            var data = SaveService.MatchDataHandler.LoadMatchData();
            data.AddStat(StatType.Wins, 1);
            SaveService.MatchDataHandler.SaveMatchData(data);
            
            var playerData = SaveService.ProfileDataHandler.LoadProfileData();
            playerData ??= new PlayerData();
            
            var levelByPoints = playerLevelInfoList.GetLevelByPoints(playerData.AllTimePoints, out _);
            playerData.AllTimePoints += playerLevelInfoList.Levels[levelByPoints - 1].SurplusForWin;
            
            SaveService.ProfileDataHandler.SaveProfileData();
            
            _currencyService.AddGoldCoins(_currencyConfig.GoldForLevelWin);
        }
        
        private void OnDefeat()
        {            
            Open();
            var bannerConfig = _bannerConfig.Banners.FirstOrDefault(b => b.Type == MatchEndControllerBannerType.Defeat);
            Text.text = bannerConfig.Text.GetLocalizedString();
            BannerImage.sprite = bannerConfig.Banner;
            
            var data = SaveService.MatchDataHandler.LoadMatchData();
            data.AddStat(StatType.Losses, 1);
            SaveService.MatchDataHandler.SaveMatchData(data);
            
            _currencyService.AddGoldCoins(_currencyConfig.GoldForLevelDefeat);
        }
        
        private void OnDraw()
        {
            Open();
            var bannerConfig = _bannerConfig.Banners.FirstOrDefault(b => b.Type == MatchEndControllerBannerType.Draw);
            Text.text = bannerConfig.Text.GetLocalizedString();
            BannerImage.sprite = bannerConfig.Banner;
            
            var data = SaveService.MatchDataHandler.LoadMatchData();
            data.AddStat(StatType.Draws, 1);
            SaveService.MatchDataHandler.SaveMatchData(data);
        }
        

        protected virtual void Awake()
        {
            MenuButton.onClick.AddListener(ToMenu);
        }

        protected virtual void ToMenu()
        {
            SceneLoader.LoadMainMenu();
        }

        protected virtual void Open()
        {
            Screen.gameObject.SetActive(true);
        }
    }
}