using System.Collections;
using BG_Games.Card_Game_Core.Player.Mulligan;
using BG_Games.Card_Game_Core.Systems;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    class MulliganTurnState:TurnState
    {
        private int _mulliganTime;
        private MulliganHandler _mulliganHandler;

        public MulliganTurnState(MatchSettings matchSettings,MulliganHandler mulliganHandler)
        {
            _mulliganTime = matchSettings.MulliganTime;
            _mulliganHandler = mulliganHandler;
        }

        public override void Enter()
        {
            _mulliganHandler.Activate();
            Player.StartCoroutine(Timer());
        }

        public override void Exit()
        {
            _mulliganHandler.Deactivate();            
            _mulliganHandler.AddCardsToStartHand();
        }

        private void MulliganTimeOut()
        {
            if (Player.ScheduledStart)
            {
                Player.NextTurn();
            }
            else
            {
                StateMachine.SwitchState<WaitingTurnState>();
            }
        }

        public override void NextTurn()
        {
            StateMachine.SwitchState<FirstPreTurnState>();
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(_mulliganTime);

            MulliganTimeOut();
        }
    }
}
