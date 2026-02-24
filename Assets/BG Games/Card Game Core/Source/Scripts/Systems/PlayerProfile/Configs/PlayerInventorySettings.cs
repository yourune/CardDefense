using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile
{
    [CreateAssetMenu(fileName = "Player Inventory Config", menuName = "ScriptableObjects/Settings/Player Inventory Config")]
    public class PlayerInventorySettings : ScriptableObject
    {
        [field: SerializeField] public List<DeckCardInfo> DefaultCards { get; private set; }
    }
}
