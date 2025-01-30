using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _Scripts.Gameplay.RollQueue;
using _Scripts.Netcore.Data.Attributes;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.RPCSystem;
using _Scripts.Netcore.RPCSystem.ProcessorsData;
using _Scripts.Netcore.Runner;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.WinSystem
{
    public class WinPresenter : NetworkBehaviour
    {
        private readonly MethodInfo _methodInfo = typeof(WinPresenter).GetMethod(nameof(Check));
        private readonly MethodInfo _restartMethodInfo = typeof(WinPresenter).GetMethod(nameof(Restart));
        private readonly MethodInfo _restartServerMethodInfo = typeof(WinPresenter).GetMethod(nameof(RestartServerRpc));

        [SerializeField] private WinView _winView;

        private IWinService _winService;
        private IQueueService _queueService;
        private INetworkRunner _networkRunner;

        [Inject]
        public void Initialize(IWinService winService,
            IQueueService queueService,
            INetworkRunner networkRunner)
        {
            _winService = winService;
            _queueService = queueService;
            _networkRunner = networkRunner;

            _queueService.OnTurnOver += CheckWin;
            RPCInvoker.RegisterRPCInstance<WinPresenter>(this);
        }

        private void CheckWin()
        {
            int winnerId = _winService.GetWinner(out bool isTied, out List<int> tiedPlayers);

            if (!_networkRunner.IsServer) 
                return;
            
            RPCInvoker.InvokeBehaviourRPC<WinPresenter>(this, _methodInfo,
                NetProtocolType.Tcp, winnerId, isTied, tiedPlayers);
            
            Check(winnerId, isTied, tiedPlayers);
        }

        [ClientRPC]
        public async void Check(int id, bool isTied, List<int> tiedPlayers)
        {
            if (!isTied)
                SetText(_networkRunner.PlayerId == id ? "Win" : "Loose");
            else
            {
                if (tiedPlayers.Any(player => _networkRunner.PlayerId == player))
                {
                    SetText("Tied");
                    return;
                }

                SetText("Loose");
            }

            await UniTask.Delay(3000);

            if (_networkRunner.IsServer)
            {
                RestartServerRpc();
                return;
            }

            RPCInvoker.InvokeBehaviourRPC<WinPresenter>(this, _restartServerMethodInfo, NetProtocolType.Tcp);
        }

        private void SetText(string text)
        {
            _winView.gameObject.SetActive(true);
            _winView.WinText.text = text;
        }
        
        [ClientRPC]
        public void Restart()
        {
            Debug.Log("restart");
            _winView.gameObject.SetActive(false);
        }

        [ServerRPC]
        public void RestartServerRpc()
        {
            Restart();
            RPCInvoker.InvokeBehaviourRPC<WinPresenter>(this, _restartMethodInfo, NetProtocolType.Tcp);
        }
    }
}