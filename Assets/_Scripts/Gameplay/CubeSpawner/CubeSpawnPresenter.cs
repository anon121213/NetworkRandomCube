using _Scripts.Gameplay.CubeRoller;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.CubeSpawner
{
    public class CubeSpawnPresenter : MonoBehaviour
    {
        [SerializeField] private CubeSpawnView _cubeSpawnView;
        
        private ICubeRoller _cubeRoller;

        [Inject]
        public void Initialize(ICubeRoller cubeSpawner)
        {
            _cubeRoller = cubeSpawner;
            _cubeSpawnView.SpawnButton.onClick.AddListener(_cubeRoller.ThrowDice);
        }

        private void OnDestroy() => 
            _cubeSpawnView.SpawnButton.onClick.RemoveListener(_cubeRoller.ThrowDice);
    }
}