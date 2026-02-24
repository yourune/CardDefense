
using UnityEngine;

namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    class MatchEndTurnState:TurnState
    {
        public override void Enter()
        {
            Debug.Log("Match end state");
        }

        public override void Exit()
        {
        }
    }
}
