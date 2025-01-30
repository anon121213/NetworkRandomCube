using System;

namespace _Scripts.Infrastructure.LobbySystem.Data
{
    [Serializable]
    public class Lobby
    {
        public int lobbyId;  
        public string ip;
        public int tcpPort;
        public int udpPort;
    }
}