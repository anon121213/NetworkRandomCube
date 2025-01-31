using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Infrastructure.ConnectWindow.Lobbies
{
    public class LobbyView : MonoBehaviour
    {
        [field: SerializeField] public Button ConnectButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LobbyName { get; private set; }

        public void ChangeName(string name) => 
            LobbyName.text = name;
    }
}