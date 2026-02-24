using BG_Games.Card_Game_Core.Tools;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [CreateAssetMenu(fileName = "Avatar Config",menuName = "ScriptableObjects/Settings/Avatar Config")]
    public class ProfileAvatarConfig : ScriptableObject
    {
        [field: SerializeField, ScriptableObjectId] public string Id { get; private set; }
        [field: SerializeField] public Sprite Avatar { get; private set; }
    }
}