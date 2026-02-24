using System;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [Serializable]
    public struct PlayerLevelInfo
    {
        [field: SerializeField] public int MinValue { get; private set; }
        [field: SerializeField] public int MaxValue { get; private set; }
        [field: SerializeField] public int SurplusForWin { get; private set; }
    }
} 