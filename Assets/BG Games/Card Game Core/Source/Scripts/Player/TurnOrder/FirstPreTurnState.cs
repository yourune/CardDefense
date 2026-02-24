namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    class FirstPreTurnState:TurnState
    {
        private PlayerEnergy _playerEnergy;
        private DrawCard _drawCard;

        public FirstPreTurnState(PlayerEnergy playerEnergy, DrawCard drawCard)
        {
            _playerEnergy = playerEnergy;
            _drawCard = drawCard;
        }

        public override void Enter()
        {
            _playerEnergy.GainEnergy();

            StateMachine.SwitchState<MainTurnState>();
        }

        public override void Exit()
        {
        }
    }
}
