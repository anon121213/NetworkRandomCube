using System.Net;
using _Scripts.Infrastructure.SceneLoader;
using _Scripts.Netcore.Data.ConnectionData;
using _Scripts.Netcore.Runner;
using UnityEngine;
using VContainer;

namespace _Scripts.Infrastructure.ConnectWindow.Lobbies
{
    public class LobbyPresenter : MonoBehaviour
    {
        [SerializeField] private LobbyView _lobbyView;
        
        private INetworkRunner _networkRunner;

        private string _ip;
        private int _tcpPort;
        private int _udpPort;
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Initialize(INetworkRunner networkRunner,
            ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _networkRunner = networkRunner;
            _lobbyView.ConnectButton.onClick.AddListener(Connect);
        }

        public void InitNetworkData(string ip, int tcpPort, int udpPort)
        {
            _ip = ip;
            _tcpPort = tcpPort;
            _udpPort = udpPort;
        }

        private async void Connect()
        {
            IPAddress.TryParse(_ip, out IPAddress ipAddress);
            
            ConnectClientData clientData = new ConnectClientData
            {
                Ip = ipAddress,
                TcpPort = _tcpPort,
                UdpPort = _udpPort
            };
            
            await _networkRunner.StartClient(clientData);
            //await _sceneLoader.Load("MainScene");
            
            gameObject.SetActive(false);
        }
    }
}