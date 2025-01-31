using System;
using _Scripts.Gameplay.CubeComponent;
using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.CubeSpawner;
using _Scripts.Gameplay.RollQueue;
using _Scripts.Gameplay.WinSystem;
using _Scripts.Netcore.Runner;

namespace _Scripts.Infrastructure.StateMachine.States
{
    public class CubeSetupperState : IState, IDisposable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly INetworkRunner _networkRunner;
        private readonly IWinService _winService;
        private readonly ICubeSpawner _cubeSpawner;
        private readonly ICubeRoller _cubeRoller;
        private readonly IQueueService _queueService;
        private readonly ICubeRollerChecker _cubeRollerChecker;

        public CubeSetupperState(IGameStateMachine gameStateMachine,
            INetworkRunner networkRunner,
            IWinService winService,
            ICubeSpawner cubeSpawner,
            ICubeRoller cubeRoller,
            IQueueService queueService,
            ICubeRollerChecker cubeRollerChecker)
        {
            _gameStateMachine = gameStateMachine;
            _networkRunner = networkRunner;
            _winService = winService;
            _cubeSpawner = cubeSpawner;
            _cubeRoller = cubeRoller;
            _queueService = queueService;
            _cubeRollerChecker = cubeRollerChecker;
        }
        
        public async void Enter()
        {
            if (!_networkRunner.IsServer)
            {
                _gameStateMachine.Enter<BotCreatorState>();
                return;
            }
            
            _cubeRollerChecker.OnChangeDiceValue += _winService.ChangeScores;
            _cubeRollerChecker.OnChangeDiceValue += ChangeTurn;
            
            _winService.AddPlayer(0);
            var cube = await _cubeSpawner.Spawn();
            _cubeRoller.Initialize(cube);
            _cubeRollerChecker.Initialize(cube);
            
            _gameStateMachine.Enter<BotCreatorState>();
        }

        private void ChangeTurn(int value) => 
            _queueService.ChangeTurn();

        public void Exit()
        {
        }

        public void Dispose()
        {
            _cubeRollerChecker.OnChangeDiceValue -= _winService.ChangeScores;
            _cubeRollerChecker.OnChangeDiceValue -= ChangeTurn;
        }
    }
}