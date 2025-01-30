using System;
using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.CubeSpawner;
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

        private Action _onClientStartedAction;
        private Action _onServerStartedAction;
        private Action<int> _onPlayerConnectedAction;

        public Bootstrapper(IWarmupService warmupService,
            INetworkRunner networkRunner,
            INetworkSpawner networkSpawner,
            ISceneLoader sceneLoader,
            ICubeSpawner cubeSpawner,
            ICubeRoller cubeRoller)
        {
            _warmupService = warmupService;
            _networkRunner = networkRunner;
            _networkSpawner = networkSpawner;
            _sceneLoader = sceneLoader;
            _cubeSpawner = cubeSpawner;
            _cubeRoller = cubeRoller;
        }
        
        public async void Initialize()
        {
            await _warmupService.Warmup();

            _onClientStartedAction = LoadMainScene;
            _onServerStartedAction = LoadMainScene;
            _onPlayerConnectedAction = _ => Sync();

            _networkRunner.OnClientStarted += _onClientStartedAction;
            _networkRunner.OnServerStarted += _onServerStartedAction;
            _networkRunner.OnPlayerConnected += _onPlayerConnectedAction;
        }

        private async void LoadMainScene()
        {
            await _sceneLoader.Load("MainScene");

            if (!_networkRunner.IsServer) 
                return;
            
            var cube = await _cubeSpawner.Spawn();
            _cubeRoller.Initialize(cube);
        }

        private void Sync() => 
            _networkSpawner.Sync();

        public void Dispose()
        {
            _networkRunner.OnClientStarted -= _onClientStartedAction;
            _networkRunner.OnServerStarted -= _onServerStartedAction;
            _networkRunner.OnPlayerConnected -= _onPlayerConnectedAction;
        }
    }
}
