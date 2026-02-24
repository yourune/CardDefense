using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [CreateAssetMenu(fileName = "Avatar Config List",menuName = "ScriptableObjects/Settings/Avatar Config List")]
    public class ProfileAvatarConfigList : ScriptableObject
    {
        [field: SerializeField] public List<ProfileAvatarConfig> Avatars { get; private set; }
        
        public ProfileAvatarConfig GetAvatarById(string id)
        {
            var avatar = Avatars.Find(avatar => avatar.Id == id);
            
            if (avatar == null)
            {
                avatar = Avatars[0];
            }
            
            return avatar;
        }
    }
}