namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    class WaitingTurnState:TurnState
    {
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void NextTurn()
        {
            StateMachine.SwitchState<PreTurnState>();
        }
    }
}
