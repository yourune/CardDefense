using System;
using BG_Games.Card_Game_Core.Cards.Info;
using UnityEngine;

namespace BG_Games.Card_Game_Core.UI.DeckAssembly
{

    [CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Settings/HeroPreview")]
    public class HeroPreviewConfig:ScriptableObject
    {
        [Serializable]
        public struct Record
        {
            [field: SerializeField] public CardRace Race { get; set; }
            [field: SerializeField] public Sprite Frame { get; set; }
            [field: SerializeField] public string RaceLabel { get; set; }
            [field: SerializeField] public Color RaceTextColor { get; set; }
        }

        [field: SerializeField] public Record[] Records { get; set; }
    }
}
