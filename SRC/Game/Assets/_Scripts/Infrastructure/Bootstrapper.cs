using System;
using _Scripts.Infrastructure.StateMachine;
using _Scripts.Infrastructure.StateMachine.StateFactory;
using _Scripts.Infrastructure.StateMachine.States;
using VContainer.Unity;

namespace _Scripts.Infrastructure
{
    public class Bootstrapper : IInitializable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStatesFactory _statesFactory;

        private Action<int> _onPlayerConnectedAction;
        private Action<int> _onChangeDice;


        public Bootstrapper(IGameStateMachine stateMachine,
            IStatesFactory statesFactory)
        {
            _stateMachine = stateMachine;
            _statesFactory = statesFactory;
        }

        public void Initialize()
        {
            RegisterStates();
            _stateMachine.Enter<BootstrapState>();
        }

        private void RegisterStates()
        {
            _stateMachine.RegisterState<BootstrapState>(_statesFactory.CreateSystem<BootstrapState>());
            _stateMachine.RegisterState<LoadSceneState>(_statesFactory.CreateSystem<LoadSceneState>());
            _stateMachine.RegisterState<CubeSetupperState>(_statesFactory.CreateSystem<CubeSetupperState>());
            _stateMachine.RegisterState<BotCreatorState>(_statesFactory.CreateSystem<BotCreatorState>());
        }
    }
}