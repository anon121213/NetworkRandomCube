using _Scripts.Infrastructure.Factory;
using _Scripts.Infrastructure.WarmupSystem;

namespace _Scripts.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ILobbyPanelFactory _lobbyPanelFactory;
        private readonly IWarmupService _warmupService;

        public BootstrapState(IGameStateMachine gameStateMachine,
            ILobbyPanelFactory lobbyPanelFactory,
            IWarmupService warmupService)
        {
            _gameStateMachine = gameStateMachine;
            _lobbyPanelFactory = lobbyPanelFactory;
            _warmupService = warmupService;
        }
        
        public async void Enter()
        {
            _warmupService.RegisterWarmupService(_lobbyPanelFactory);
            await _warmupService.Warmup();
            
            _gameStateMachine.Enter<LoadSceneState>();
        }

        public void Exit()
        {
        }
    }
}