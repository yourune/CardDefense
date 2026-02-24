using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    [CreateAssetMenu(fileName = "Deck Settings",menuName = "ScriptableObjects/Settings/Deck")]
    public class DeckSettings : ScriptableObject
    {
        [field: SerializeField] public int Size { get; private set; }
        [field: SerializeField] public int MaxDuplicates { get; private set; }
    }
}