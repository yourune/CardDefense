using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class AvatarSelectionView: MonoBehaviour
    {
        [SerializeField] private ProfileAvatarConfigList _profileAvatarConfigList;
        [SerializeField] private AvatarVariant _avatarVariantPrefab;
        [SerializeField] private RectTransform _avatarContainer;
        [SerializeField] private Image _image;

        public void Init()
        {
            foreach (var avatar in _profileAvatarConfigList.Avatars)
            {
                var avatarVariant = Instantiate(_avatarVariantPrefab, _avatarContainer);
                avatarVariant.Init(avatar);
            }

            UpdateImage();

            EventBus.Subscribe<AvatarSelectedEvent>(OnAvatarSelected);
            EventBus.Subscribe<PlayerDataReloadEvent>(OnPlayerDataUpdate);
        }

        public void DeInit()
        {
            EventBus.Unsubscribe<AvatarSelectedEvent>(OnAvatarSelected);
            EventBus.Unsubscribe<PlayerDataReloadEvent>(OnPlayerDataUpdate);
        }

        private void OnPlayerDataUpdate(PlayerDataReloadEvent data)
        {
            UpdateImage();
        }

        private void UpdateImage()
        {
            var currentAvatarId = SaveService.ProfileDataHandler.LoadProfileData().AvatarImageId;
            var currentAvatar = _profileAvatarConfigList.GetAvatarById(currentAvatarId);
            if (currentAvatar != null)
            {
                _image.sprite = currentAvatar.Avatar;
            }
        }

        private void OnAvatarSelected(AvatarSelectedEvent eventData)
        {
            _image.sprite = eventData.ProfileAvatarConfig.Avatar;
        }
    }
}