using BG_Games.Card_Game_Core.Systems.EventsBus;
using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    public class AvatarVariant: MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
            
        private ProfileAvatarConfig _profileAvatarConfig;
        
        public void Init(ProfileAvatarConfig profileAvatarConfig)
        {
            _profileAvatarConfig = profileAvatarConfig;
            _image.sprite = profileAvatarConfig.Avatar;
            
            _button.onClick.AddListener(OnSelected);
        }

        private void OnSelected()
        {
            var playerProfileData = SaveService.ProfileDataHandler.LoadProfileData();
            playerProfileData.AvatarImageId = _profileAvatarConfig.Id;
            SaveService.ProfileDataHandler.SaveProfileData();
            
            
            EventBus.Publish(new AvatarSelectedEvent(_profileAvatarConfig));
        }
    }
}