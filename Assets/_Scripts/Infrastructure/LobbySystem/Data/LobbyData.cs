using System;

namespace _Scripts.Infrastructure.LobbySystem
{
    [Serializable]
    public class LobbyData
    {
        public string ip;
        public int tcpPort;
        public int udpPort;
    }
}