using BG_Games.Card_Game_Core.Player;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Systems
{
    public class GameTable : MonoBehaviour
    {
        [field: SerializeField] public TableSide BottomTableSide { get; private set; }
        [field: SerializeField] public TableSide TopTableSide { get; private set; }

        public TableSide GetMyTableSide(PlayerId playerId)
        {
            return playerId == PlayerId.Player1 ? BottomTableSide : TopTableSide;
        }

        public TableSide GetOpponentTableSide(PlayerId playerId)
        {
            return playerId == PlayerId.Player1 ? TopTableSide : BottomTableSide;
        }
    }
}