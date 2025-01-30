using System.Reflection;
using _Scripts.Infrastructure.ConnectWindow.Lobbies;
using _Scripts.Infrastructure.ConnectWindow.Lobbys;
using _Scripts.Infrastructure.Factory;
using _Scripts.Infrastructure.LobbySystem;
using _Scripts.Netcore.Data.Attributes;
using _Scripts.Netcore.Data.ConnectionData;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.RPCSystem;
using _Scripts.Netcore.Runner;
using UnityEngine;
using VContainer;

namespace _Scripts.Infrastructure.ConnectWindow
{
    public class ConnectWindowPresenter : MonoBehaviour
    {
        [SerializeField] private ConnectView _connectView;
        
        private INetworkRunner _networkRunner;
        private ILobbyManager _lobbyManager;
        private ILobbyPanelFactory _lobbyPanelFactory;

        [Inject]
        public void Initialize(INetworkRunner networkRunner,
            ILobbyManager lobbyManager,
            ILobbyPanelFactory lobbyPanelFactory)
        {
            _networkRunner = networkRunner;
            _lobbyManager = lobbyManager;
            _lobbyPanelFactory = lobbyPanelFactory;
            
            _connectView.CreateLobbyButton.onClick.AddListener(CreateLobby);
            _connectView.SearchLobbiesButton.onClick.AddListener(SearchLobbies);
        }

        private async void SearchLobbies()
        {
            var lobbies = await _lobbyManager.GetLobbiesAsync();

            foreach (var lobby in lobbies)
            {
                GameObject panel = await _lobbyPanelFactory.CreateLobbyPanel(_connectView.LobbysContainer);
                var view = panel.GetComponent<LobbyView>();
                var presenter = panel.GetComponent<LobbyPresenter>();
                
                presenter.InitNetworkData(lobby.ip, lobby.tcpPort, lobby.udpPort);
                view.ChangeName(lobby.ip);
            }
        }

        private async void CreateLobby()
        {
            ConnectServerData serverData = new ConnectServerData
            {
                MaxClients = 2,
                TcpPort = 5055,
                UdpPort = 5057
            };

            await _networkRunner.StartServer(serverData);

            _lobbyManager.AddLobbyAsync(_networkRunner.ServerIp.ToString(), serverData.TcpPort, serverData.UdpPort);
        }
    }
}