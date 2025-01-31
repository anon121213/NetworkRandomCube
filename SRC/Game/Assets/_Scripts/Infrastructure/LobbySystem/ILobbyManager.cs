using System.Collections.Generic;
using _Scripts.Infrastructure.LobbySystem.Data;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.LobbySystem
{
    public interface ILobbyManager
    {
        UniTask AddLobbyAsync(string ip, int tcpPort, int udpPort);
        UniTask<List<Lobby>> GetLobbiesAsync();
    }
}