using BG_Games.Card_Game_Core.Systems;

namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    public class MainTurnState:TurnState
    {
        private PlayerHand _hand;
        private TableSide _tableSide;
        private TurnSwitch _turnSwitch;
        private PlayerHero _hero;

        public MainTurnState(PlayerId player, GameTable table, PlayerHand playerHand,PlayerHero hero,TurnSwitch turnSwitch)
        {
            _hand = playerHand;
            _tableSide = table.GetMyTableSide(player);
            _turnSwitch = turnSwitch;
            _hero = hero;
        }

        public override void Enter()
        {            
            _hand.SetLockActions(false);
            _tableSide.SetLockActions(false);
            _tableSide.NextTurn();
            _hero.Card.SetLockActions(false);
            _hero.Card.NextTurn();
        }

        public override void Exit()
        {
            _hero.Card.EndTurn();
            _tableSide.EndTurn();

            _hand.MaxHandRule();

            _hand.SetLockActions(true);
            _tableSide.SetLockActions(true);
            _hero.Card.SetLockActions(true);

            _turnSwitch.TurnEnded();
        }

        public override void EndTurn()
        {
            StateMachine.SwitchState<WaitingTurnState>();
        }
    }
}
