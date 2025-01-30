using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.RollQueue;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.CubeSpawner
{
    public class CubeSpawnPresenter : MonoBehaviour
    {
        [SerializeField] private CubeSpawnView _cubeSpawnView;
        
        private ICubeRoller _cubeRoller;
        private IQueueService _queueService;

        [Inject]
        public void Initialize(ICubeRoller cubeSpawner,
            IQueueService queueService)
        {
            _cubeRoller = cubeSpawner;
            _queueService = queueService;
            _cubeSpawnView.SpawnButton.onClick.AddListener(Roll);
        }

        private void Roll()
        {
            if (!_queueService.CheckTurn())
                return;
            
            _cubeRoller.Roll();
        }
        
        private void OnDestroy() => 
            _cubeSpawnView.SpawnButton.onClick.RemoveListener(_cubeRoller.Roll);
    }
}