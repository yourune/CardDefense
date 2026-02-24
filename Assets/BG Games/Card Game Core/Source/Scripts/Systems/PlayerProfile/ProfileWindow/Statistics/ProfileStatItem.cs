using TMPro;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics
{
    public class ProfileStatItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text _statKey;
        [SerializeField] private TMP_Text _statValue;
        
        public void Init(string key, string value)
        {
            _statKey.text = key;
            _statValue.text = value;
        }
    }
}