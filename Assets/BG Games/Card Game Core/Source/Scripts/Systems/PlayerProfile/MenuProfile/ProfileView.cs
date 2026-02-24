using System.Collections;
using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class ProfileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Slider _levelProgressBar;
        [SerializeField] private Image _avatar;
        [SerializeField] private ProfileAvatarConfigList _profileAvatarConfigList;
        [FormerlySerializedAs("_profileLevelConfigList")] [SerializeField] private PlayerLevelInfoList playerLevelInfoList;

        private PlayerData _playerData;
        private IEnumerator _levelProgressSequence;

        private void Awake()
        {
            EventBus.Subscribe<NameUpdatedEvent>(OnNameUpdated);
            EventBus.Subscribe<AvatarSelectedEvent>(OnAvatarSelected);
            EventBus.Subscribe<PlayerDataReloadEvent>(UpdatePlayerData);
        }

        private void Start()
        {
            _playerData = SaveService.ProfileDataHandler.LoadProfileData();

            UpdateView();
        }

        private void UpdatePlayerData(PlayerDataReloadEvent data)
        {
            _playerData = data.Data;
            UpdateView();
        }

        private void UpdateView()
        {
            _avatar.sprite = _profileAvatarConfigList.GetAvatarById(_playerData.AvatarImageId).Avatar;
            _name.text = _playerData.Name;

            int allTimePoints = _playerData.AllTimePoints;
            int playerLevel = playerLevelInfoList.GetLevelByPoints(allTimePoints, out _);
            _level.text = playerLevel + PlayerProfileConsts.LevelText; 
            
            StartCoroutine(playerLevelInfoList.SetProgressForCurrentLevel(_levelProgressBar, allTimePoints));
        }

        private void OnAvatarSelected(AvatarSelectedEvent eventData)
        {
            _avatar.sprite = eventData.ProfileAvatarConfig.Avatar;
            _playerData.AvatarImageId = eventData.ProfileAvatarConfig.Id;
        }

        private void OnNameUpdated(NameUpdatedEvent eventData)
        {
            _name.text = eventData.Name;
            _playerData.Name = eventData.Name;

            SaveService.ProfileDataHandler.SaveProfileData();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<NameUpdatedEvent>(OnNameUpdated);
            EventBus.Unsubscribe<AvatarSelectedEvent>(OnAvatarSelected);
            EventBus.Unsubscribe<PlayerDataReloadEvent>(UpdatePlayerData);
        }
    }
}