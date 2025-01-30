using System;
using System.Reflection;
using _Scripts.Netcore.Data.Attributes;
using _Scripts.Netcore.NetworkComponents.NetworkVariableComponent;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.RPCSystem;
using _Scripts.Netcore.RPCSystem.ProcessorsData;
using _Scripts.Netcore.Runner;

namespace _Scripts.Gameplay.RollQueue
{
    public class QueueService : NetworkService, IQueueService
    {
        private readonly INetworkRunner _networkRunner;
        private readonly MethodInfo _methodInfo = typeof(QueueService).GetMethod(nameof(Change));

        public NetworkVariable<int> TurnIndex { get; set; } = new("TurnIndex", 0);

        public event Action OnTurnOver;
        
        public QueueService(INetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
            RPCInvoker.RegisterRPCInstance<QueueService>(this);
        }

        public bool CheckTurn() =>
            _networkRunner.PlayerId == TurnIndex.Value;

        public void ChangeTurn()
        {
            if (_networkRunner.IsServer)
                Change();
            else
                RPCInvoker.InvokeServiceRPC<QueueService>(this, _methodInfo, NetProtocolType.Tcp);
        }

        [ServerRPC]
        public void Change()
        {
            if (TurnIndex.Value >= _networkRunner.MaxClients)
            {
                TurnIndex.Value = 0;
                OnTurnOver?.Invoke();
                return;
            }

            TurnIndex.Value++;
        }
    }

    public interface IQueueService
    {
        NetworkVariable<int> TurnIndex { get; set; }
        bool CheckTurn();
        void ChangeTurn();
        event Action OnTurnOver;
    }
}