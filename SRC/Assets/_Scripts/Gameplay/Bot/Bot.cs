using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.RollQueue;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.Bot
{
    public class Bot : IBot
    {
        private readonly ICubeRoller _roller;
        private readonly IQueueService _queueService;
        private readonly int _id;

        public Bot(ICubeRoller roller,
            IQueueService queueService,
            int id)
        {
            _roller = roller;
            _queueService = queueService;
            _id = id;
        }

        private async void Roll(int obj)
        {
            await UniTask.Delay(1500);
            
            if (_queueService.TurnIndex.Value != _id)
                return;
            
            _roller.Roll();
        }

        public void EnableBot() => 
            _queueService.TurnIndex.OnValueChanged += Roll;

        public void DisableBot() => 
            _queueService.TurnIndex.OnValueChanged -= Roll;
    }

    public interface IBot
    {
        void EnableBot();
        void DisableBot();
    }
}