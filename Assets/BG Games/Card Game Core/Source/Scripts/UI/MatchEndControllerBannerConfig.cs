using System;
using UnityEngine;
using UnityEngine.Localization;

namespace BG_Games.Card_Game_Core.UI
{
    [CreateAssetMenu(fileName = "Match End Controller Banner Config", menuName = "ScriptableObjects/Settings/Match End Controller Banner Config")]
    public class MatchEndControllerBannerConfig : ScriptableObject
    {
        [field: SerializeField] public MatchEndControllerBanner[] Banners { get; private set; }   
    }
    
    public enum MatchEndControllerBannerType
    {
        Win,
        Defeat, 
        Draw
    }
    
    [Serializable]
    public struct MatchEndControllerBanner
    {
        public MatchEndControllerBannerType Type;
        public Sprite Banner;
        public LocalizedString Text;
    }
}