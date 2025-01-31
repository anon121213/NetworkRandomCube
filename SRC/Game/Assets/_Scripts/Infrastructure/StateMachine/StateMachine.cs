using System;
using System.Collections.Generic;

namespace _Scripts.Infrastructure.StateMachine
{
    public class StateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states = new Dictionary<Type, IExitableState>();
        private IExitableState _activeState;

        public void RegisterState<TState>(IExitableState state) where TState : IExitableState =>
            _states.Add(typeof(TState), state);

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }

    public interface IGameStateMachine : IStateMachine
    {
        
    }

    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void RegisterState<TState>(IExitableState state) where TState : IExitableState;
    }

    public interface IState: IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    public interface IExitableState
    {
        void Exit();
    }
}