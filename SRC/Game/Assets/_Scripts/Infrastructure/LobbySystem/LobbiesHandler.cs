using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using _Scripts.Infrastructure.LobbySystem.Data;

namespace _Scripts.Infrastructure.LobbySystem
{
    public class LobbiesHandler : ILobbyManager, IDisposable
    {
        private const string SERVER_URL = "http://185.125.103.42:3000";
        
        private readonly List<Lobby> _lobbies = new();

        private Lobby _currentLobby;
        
        public async UniTask AddLobbyAsync(string ip, int tcpPort, int udpPort)
        {
            var lobbyData = new LobbyData { ip = ip, tcpPort = tcpPort, udpPort = udpPort };
            string jsonData = JsonUtility.ToJson(lobbyData);

            using UnityWebRequest request = new UnityWebRequest($"{SERVER_URL}/create-lobby", "POST");
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData); 
            
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Лобби создано: {request.downloadHandler.text}");
                    
                _currentLobby = new Lobby
                {
                    ip = ip,
                    tcpPort = tcpPort,
                    udpPort = udpPort
                };
            }
            else
            {
                Debug.LogError($"Ошибка при создании лобби: {request.error}");
            }
        }

        public async UniTask<List<Lobby>> GetLobbiesAsync()
        {
            using UnityWebRequest request = UnityWebRequest.Get($"{SERVER_URL}/lobbies");
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                LobbyListWrapper wrapper = JsonUtility.FromJson<LobbyListWrapper>($"{{\"lobbies\":{jsonResponse}}}");

                _lobbies.Clear();
                    
                foreach (var lobby in wrapper.lobbies) 
                    _lobbies.Add(lobby);
                    
                return _lobbies;
            }

            Debug.LogError($"Ошибка при получении списка лобби: {request.error}");
            return null;
        }
        
        private async UniTask DeleteLobbyAsync(string ip, int tcpPort, int udpPort)
        {
            string url = $"{SERVER_URL}/delete-lobby?ip={ip}&tcpPort={tcpPort}&udpPort={udpPort}";

            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Лобби удалено: {request.downloadHandler.text}");

                _lobbies.RemoveAll(lobby => lobby.ip == ip && lobby.tcpPort == tcpPort && lobby.udpPort == udpPort);
            }
            else
            {
                Debug.LogError($"Ошибка при удалении лобби: {request.error}");
            }
        }

        public void Dispose()
        {
            if (_currentLobby != null)
            {
                Debug.Log(_currentLobby.ip);
                DeleteLobbyAsync(_currentLobby.ip, _currentLobby.tcpPort, _currentLobby.udpPort).Forget();
            }
        }
    }
}
