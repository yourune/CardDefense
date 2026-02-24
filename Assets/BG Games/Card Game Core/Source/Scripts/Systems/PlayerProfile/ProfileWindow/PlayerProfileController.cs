using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class PlayerProfileController : MonoBehaviour
    {
        [SerializeField, Header("Rects")] private RectTransform _screen;
        [SerializeField] private RectTransform _confirmDeleteProfileWindow;
        [SerializeField] private ProfileStatisticsView _profileStatisticsView;
        [SerializeField] private AvatarSelectionView _avatarContainer;

        [SerializeField, Header("Stats")] private TMP_Text _minRating;
        [SerializeField] private TMP_Text _maxRating;
        [SerializeField] private Slider _levelSlider;
        [SerializeField] private TMP_Text _level;

        [FormerlySerializedAs("_profileLevelConfigList")] [SerializeField, Header("Configs")] private PlayerLevelInfoList playerLevelInfoList;
        
        [SerializeField, Header("Buttons")] private Button _closeButton;
        [Space(10), SerializeField] private TMP_InputField _nameInputField;
        
        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
            
            _nameInputField.onEndEdit.AddListener(OnNameChanged);
        }

        private void OnNameChanged(string arg0)
        {
            if(arg0.Length == 0)
            {
                arg0 = PlayerProfileConsts.DefaultProfileName;
            }
             
            EventBus.Publish(new NameUpdatedEvent(arg0));
        }

        public void Open()
        {
            _screen.gameObject.SetActive(true);
        }

        private void Start()
        {
            _profileStatisticsView.Init();
            _avatarContainer.Init();

            UpdateView();
        }

        private void OnDestroy()
        {
            _avatarContainer.DeInit();
        }

        private void Close()
        {
            _avatarContainer.gameObject.SetActive(false);
            _screen.gameObject.SetActive(false);
        }
        
        public void ShowAvatarSelection()
        {
            _avatarContainer.gameObject.SetActive(_avatarContainer.gameObject.activeSelf == false);
        }
        
        public void DeleteProfile()
        {
            SaveService.ProfileDataHandler.DeleteProfileData(); 
            SaveService.Delete(PlayerProfileConsts.MatchDataKey);
            _nameInputField.text = PlayerProfileConsts.DefaultProfileName;
            _level.text = PlayerProfileConsts.DefaultPlayerLevel + PlayerProfileConsts.LevelText;
            
            _avatarContainer.gameObject.SetActive(false);
            
            EventBus.Publish(new DeleteProfileEvent());
            UpdateView();
        }

        public void ShowConfirmDeleteProfile(bool show)
        {
            _confirmDeleteProfileWindow.gameObject.SetActive(show);
        }

        private void UpdateView()
        {
            PlayerData playerProfileData = SaveService.ProfileDataHandler.LoadProfileData();
            _nameInputField.text = playerProfileData.Name;
            
            var levelByPoints = playerLevelInfoList.GetLevelByPoints(playerProfileData.AllTimePoints, out var levelInfo);
            _level.text = levelByPoints + PlayerProfileConsts.LevelText;
            
            StartCoroutine(playerLevelInfoList.SetProgressForCurrentLevel(_levelSlider, playerProfileData.AllTimePoints));
            _minRating.text = levelInfo.MinValue.ToString();
            _maxRating.text = levelInfo.MaxValue.ToString();
        }
    }
}