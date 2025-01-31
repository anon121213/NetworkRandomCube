using System;
using _Scripts.Infrastructure.Constants;
using _Scripts.Infrastructure.SceneLoader;
using _Scripts.Netcore.Runner;

namespace _Scripts.Infrastructure.StateMachine.States
{
    public class LoadSceneState : IState, IDisposable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly INetworkRunner _networkRunner;
        private readonly ISceneLoader _sceneLoader;

        public LoadSceneState(IGameStateMachine gameStateMachine,
        INetworkRunner networkRunner,
        ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _networkRunner = networkRunner;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _networkRunner.OnClientStarted += LoadScene;
            _networkRunner.OnServerStarted += LoadScene;
        }

        private async void LoadScene()
        {
            await _sceneLoader.Load(ScenesConstants.GameSceneName);
            _gameStateMachine.Enter<CubeSetupperState>();
        }

        public  void Exit()
        {
           
        }

        public void Dispose()
        {
            _networkRunner.OnClientStarted -= LoadScene;
            _networkRunner.OnServerStarted -= LoadScene;
        }
    }
}