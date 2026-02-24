using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems.CurrencySystem
{
    [CreateAssetMenu(fileName = "Currency Config", menuName = "ScriptableObjects/Settings/CurrencyConfig")]
    public class CurrencyConfig : ScriptableObject
    {
        [field: SerializeField] private int _goldForLevelFinish;

        public int GoldForLevelWin => _goldForLevelFinish * 2;
        public int GoldForLevelDefeat => _goldForLevelFinish;
    }
}
