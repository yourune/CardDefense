using Cysharp.Threading.Tasks;

namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    class StartMatchTurnState:TurnState
    {
        private PlayerEnergy _energy;
        private DrawCard _drawCard;
        private PlayerHand _hand;
        private PlayerHero _hero;

        public StartMatchTurnState(PlayerEnergy playerEnergy,DrawCard drawCard,PlayerHand playerHand,PlayerHero hero)
        {
            _energy = playerEnergy;
            _drawCard = drawCard;
            _hand = playerHand;
            _hero = hero;
        }

        public override void Enter()
        {            
            _hero.CreateHero();
            _energy.InitEnergy();
            _drawCard.InitStartHand().Forget();
            _hand.SetLockActions(true);

            StateMachine.SwitchState<MulliganTurnState>();
        }

        public override void Exit()
        {
        }
    }
}
