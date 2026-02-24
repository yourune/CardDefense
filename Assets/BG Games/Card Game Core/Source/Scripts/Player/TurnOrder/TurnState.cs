using BG_Games.Card_Game_Core.Tools;

namespace BG_Games.Card_Game_Core.Player.TurnOrder
{
    public abstract class TurnState:IBaseState
    {
        protected StateMachine<TurnState> StateMachine;
        protected Player Player;
        public void AssignToPlayerStateMachine(StateMachine<TurnState> stateMachine,Player player)
        {
            Player = player;
            StateMachine = stateMachine;
            stateMachine.AddState(this);
        }

        public abstract void Enter();

        public abstract void Exit();

        public virtual void NextTurn(){}

        public virtual void EndTurn(){}

        public virtual void MatchEnd()
        {
            StateMachine.SwitchState<MatchEndTurnState>();
        }
    }
}
