using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.RollQueue;

namespace _Scripts.Gameplay.WinSystem
{
    public class WinService : IWinService
    {
        private readonly List<(int, int)> _playersScores = new();
        private readonly IQueueService _queueService;

        public WinService(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public void AddPlayer(int id) =>
            _playersScores.Add((id, 0));

        public void ChangeScores(int score)
        {
            int index = _queueService.TurnIndex.Value;
            if (index < 0 || index >= _playersScores.Count)
                throw new IndexOutOfRangeException($"Invalid index: {index}");

            var (playerId, _) = _playersScores[index];
            _playersScores[index] = (playerId, score);
        }

        public int GetWinner(out bool isTied, out List<int> tiedPlayers)
        {
            tiedPlayers = new List<int>();  
            if (_playersScores.Count == 0)
                throw new InvalidOperationException("No players have been added.");

            int winnerId = _playersScores[0].Item1;
            int maxScore = _playersScores[0].Item2;

            tiedPlayers.Add(0); 

            foreach (var (index, (playerId, score)) in _playersScores.Select((value, index) => (index, value)))
            {
                if (score > maxScore)
                {
                    winnerId = playerId;
                    maxScore = score;
                    tiedPlayers.Clear(); 
                    tiedPlayers.Add(index); 
                    isTied = false;  
                }
                else if (score == maxScore)
                {
                    tiedPlayers.Add(index);
                    isTied = true;
                }
            }

            isTied = tiedPlayers.Count > 1;

            return winnerId;
        }

    }

    public interface IWinService
    {
        void AddPlayer(int id);
        void ChangeScores(int score);
        int GetWinner(out bool isTied, out List<int> tiedPlayers);
    }
}