using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Card_Game_Core;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [CreateAssetMenu(fileName = "Level Info List",menuName = "ScriptableObjects/Settings/Level Info List")]
    public class PlayerLevelInfoList : ScriptableObject
    {
        [SerializeField] private float _animationDuration = 1.5f;
        [field: SerializeField] public List<PlayerLevelInfo> Levels { get; private set; }
        
        public int GetLevelByPoints(int currentPoints, out PlayerLevelInfo levelInfo)
        {
            var levelInfos = Levels.Where(l => l.MinValue <= currentPoints).ToList();
            
            if (!levelInfos.Any())
            {
                levelInfo = Levels[0];
                return 1;
            }
            
            PlayerLevelInfo maxLevel = levelInfos.Aggregate((l1, l2) => l1.MinValue > l2.MinValue ? l1 : l2);
            int levelConfigIndex = Levels.IndexOf(maxLevel);
            
            levelInfo = maxLevel;
            return levelConfigIndex == -1? 1 : levelConfigIndex + 1;
        }

        public IEnumerator SetProgressForCurrentLevel(Slider slider, int allTimePoints)
        {
            GetLevelByPoints(allTimePoints, out var levelInfo);

            slider.minValue = levelInfo.MinValue;
            slider.maxValue = levelInfo.MaxValue;
            
            return TweenAnimation.SliderTo(
                slider,
                allTimePoints,
                _animationDuration,
                TweenAnimation.Linear
            );
        }
    }
}