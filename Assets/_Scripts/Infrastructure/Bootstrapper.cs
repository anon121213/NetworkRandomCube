using System;
using _Scripts.Gameplay.CubeComponent;
using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.CubeSpawner;
using _Scripts.Gameplay.RollQueue;
using _Scripts.Infrastructure.SceneLoader;
using _Scripts.Infrastructure.WarmupSystem;
using _Scripts.Netcore.Runner;
using _Scripts.Netcore.Spawner;
using VContainer.Unity;

namespace _Scripts.Infrastructure
{
    public class Bootstrapper : IInitializable, IDisposable
    {
        private readonly IWarmupService _warmupService;
        private readonly INetworkRunner _networkRunner;
        private readonly INetworkSpawner _networkSpawner;
        private readonly ISceneLoader _sceneLoader;
        private readonly ICubeSpawner _cubeSpawner;
        private readonly ICubeRoller _cubeRoller;
        private readonly ICubeRollerChecker _cubeRollerChecker;
        private readonly IQueueService _queueService;

        private Action<int> _onPlayerConnectedAction;
        private Action<int> _onChandeDice;

        public Bootstrapper(IWarmupService warmupService,
            INetworkRunner networkRunner,
            INetworkSpawner networkSpawner,
            ISceneLoader sceneLoader,
            ICubeSpawner cubeSpawner,
            ICubeRoller cubeRoller,
            ICubeRollerChecker cubeRollerChecker,
            IQueueService queueService)
        {
            _warmupService = warmupService;
            _networkRunner = networkRunner;
            _networkSpawner = networkSpawner;
            _sceneLoader = sceneLoader;
            _cubeSpawner = cubeSpawner;
            _cubeRoller = cubeRoller;
            _cubeRollerChecker = cubeRollerChecker;
            _queueService = queueService;
        }
        
        public async void Initialize()
        {
            await _warmupService.Warmup();

            _onPlayerConnectedAction = _ => Sync();
            _onChandeDice = _ => _queueService.ChangeTurn();

            _networkRunner.OnClientStarted += LoadMainScene;
            _networkRunner.OnServerStarted += LoadMainScene;
            _networkRunner.OnPlayerConnected += _onPlayerConnectedAction;
            _cubeRollerChecker.OnChangeDiceValue += _onChandeDice;
        }

        private async void LoadMainScene()
        {
            await _sceneLoader.Load("MainScene");

            if (!_networkRunner.IsServer) 
                return;
            
            var cube = await _cubeSpawner.Spawn();
            _cubeRoller.Initialize(cube);
            _cubeRollerChecker.Initialize(cube);
        }

        private void Sync() => 
            _networkSpawner.Sync();

        public void Dispose()
        {
            _networkRunner.OnClientStarted -= LoadMainScene;
            _networkRunner.OnServerStarted -= LoadMainScene;
            _networkRunner.OnPlayerConnected -= _onPlayerConnectedAction;
            _cubeRollerChecker.OnChangeDiceValue -= _onChandeDice;
        }
    }
}
