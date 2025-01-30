using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Infrastructure.ConnectWindow
{
    public class ConnectView : MonoBehaviour
    {
        [field: SerializeField] public Button CreateLobbyButton { get; private set; }
        [field: SerializeField] public Button SearchLobbiesButton { get; private set; }
        [field: SerializeField] public Button ClouseWindownButton { get; private set; }
        [field: SerializeField] public Transform LobbysContainer { get; private set; }
    }
}