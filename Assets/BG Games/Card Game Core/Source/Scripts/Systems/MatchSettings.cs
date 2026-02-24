using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    class MatchSettings:MonoBehaviour
    {
        [SerializeField] private int _mulliganTime = 10;
        [SerializeField] private int _maxHandSize = 10;
        [SerializeField] private int _initialHandSize = 3;

        public int MulliganTime => _mulliganTime;
        public int MaxHandSize => _maxHandSize;
        public int InitialHandSize => _initialHandSize;

    }
}
