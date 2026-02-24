using BG_Games.Card_Game_Core.Systems.EventsBus;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics
{
    public static class SaveService
    {
        public static void Save<T>(T data, string key)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }
        
        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
        
        public static T Load<T>(string key, T defaultValue = default)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }
            
            var json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }

        public static class MatchDataHandler
        {
            public static MatchData LoadMatchData()
            {
                return Load(PlayerProfileConsts.MatchDataKey, new MatchData());
            }
        
            public static void SaveMatchData(MatchData matchData)
            {
                Save(matchData, PlayerProfileConsts.MatchDataKey);
            }
        }
        
        public static class ProfileDataHandler
        {
            private static PlayerData _playerData;
            
            public static PlayerData LoadProfileData()
            {
                if (_playerData == null)
                {
                    _playerData = Load(PlayerProfileConsts.ProfileDataKey, new PlayerData());
                }
                
                return _playerData;
            }
        
            public static void SaveProfileData()
            {
                if(_playerData == null) 
                    return;
                
                Save(_playerData, PlayerProfileConsts.ProfileDataKey);
            }

            public static void DeleteProfileData()
            {
                _playerData = null;
                Delete(PlayerProfileConsts.ProfileDataKey);
                EventBus.Publish(new PlayerDataReloadEvent(LoadProfileData()));
            }
        }
    }
}