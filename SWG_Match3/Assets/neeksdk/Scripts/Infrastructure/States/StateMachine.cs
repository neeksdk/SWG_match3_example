using System;
using System.Collections.Generic;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class StateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        private IState _previousState;

        public void SetupStateMachine(Dictionary<Type, IExitableState> states) =>
            _states = states;

        public void Enter<TState>() where TState : class, IState {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState {
            if (_activeState is IState activeState) {
                _previousState = activeState;
            }
            
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}
