using System;
using BG_Games.Card_Game_Core.SerializableTypes;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [Serializable]
    public class MatchData
    {
        public SerializableDictionary<StatType, int> StatTypeValues;
        
        public MatchData()
        {
            StatTypeValues = new SerializableDictionary<StatType, int>
            {
                {StatType.Wins, 0},
                {StatType.Losses, 0},
                {StatType.Draws, 0}
            };
        }
        
        public void AddStat(StatType statType, int value)
        {
            if (!StatTypeValues.TryAdd(statType, value))
            {
                StatTypeValues[statType] += value;
            }
        }
    }
    
    public enum StatType
    {
        Wins,
        Losses,
        Draws
    }
}