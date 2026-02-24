using System.Collections.Generic;
using System.Linq;

namespace BG_Games.Card_Game_Core.Tools
{
    public class StateMachine<T> where T : IBaseState
    {
        private T _currentState;
        protected List<T> States;

        public T CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                _currentState.Enter();
            }
        }

        public StateMachine()
        {
            States = new List<T>();
        }

        public void AddState(T state)
        {
            States.Add(state);
        }

        public void InitState<U>() where U : T
        {
            CurrentState = States.FirstOrDefault(state => state is U);
        }

        public void SwitchState<U>() where U : T
        {
            _currentState.Exit();
            CurrentState = States.FirstOrDefault(state => state is U);
        }

    }
}
