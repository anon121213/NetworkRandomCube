using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Bot;
using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.RollQueue;
using _Scripts.Gameplay.WinSystem;
using _Scripts.Netcore.Runner;
using UnityEngine;

namespace _Scripts.Infrastructure.StateMachine.States
{
    public class BotCreatorState : IState, IDisposable
    {
        private readonly INetworkRunner _networkRunner;
        private readonly ICubeRoller _cubeRoller;
        private readonly IQueueService _queueService;
        private readonly IWinService _winService;
        private readonly List<IBot> _bots = new();
        
        public BotCreatorState(INetworkRunner networkRunner,
            ICubeRoller cubeRoller,
            IQueueService queueService,
            IWinService winService)
        {
            _networkRunner = networkRunner;
            _cubeRoller = cubeRoller;
            _queueService = queueService;
            _winService = winService;
        }

        public void Enter()
        {
            if (!_networkRunner.IsServer)
                return;
            
            _networkRunner.OnPlayerConnected += DisableBot;
            _networkRunner.OnPlayerConnected += _winService.RemovePlayer;
            _networkRunner.OnPlayerConnected += _winService.AddPlayer;
            CreateBots();
        }

        private void CreateBots()
        {
            for (int i = 1; i < _networkRunner.MaxClients; i++)
            {
                Debug.Log($"createBot {i}");
                IBot bot = new Bot(_cubeRoller, _queueService, i);
                bot.EnableBot();
                _bots.Add(bot);
                _winService.AddPlayer(i);
            }
        }

        private void DisableBot(int id)
        {
            if (_bots.Count < id)
                return;
            
            _bots[id - 1].DisableBot();
        }

        public void Exit()
        {
            
        }

        public void Dispose()
        {
            _networkRunner.OnPlayerConnected -= DisableBot;
            _networkRunner.OnPlayerConnected -= _winService.RemovePlayer;
            _networkRunner.OnPlayerConnected -= _winService.AddPlayer;
        }
    }
}