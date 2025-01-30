using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.CubeSpawner
{
    public class CubeSpawnView : MonoBehaviour
    {
        [field: SerializeField] public Button SpawnButton { get; private set; }
    }
}