namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    public class PreTurnState : TurnState
    {
        private PlayerEnergy _playerEnergy;
        private DrawCard _drawCard;

        public PreTurnState(PlayerEnergy playerEnergy,DrawCard drawCard)
        {
            _playerEnergy = playerEnergy;
            _drawCard = drawCard;
        }

        public override async void Enter()
        {            
            _playerEnergy.GainEnergy();
            await _drawCard.DrawNextCard();

            StateMachine.SwitchState<MainTurnState>();
        }

        public override void Exit()
        {
        }
    }
}
