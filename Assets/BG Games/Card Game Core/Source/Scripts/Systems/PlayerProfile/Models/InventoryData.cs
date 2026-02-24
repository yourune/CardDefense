using System;
using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [Serializable]
    public class InventoryData
    {
        [field: SerializeField] public List<string> Cards { get; private set; } = new List<string>();
    }
}
