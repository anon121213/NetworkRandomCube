using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.RollQueue;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.CubeSpawner
{
    public class CubeRollPresenter : MonoBehaviour
    {
        [SerializeField] private CubeRollView _cubeSpawnView;
        
        private ICubeRoller _cubeRoller;
        private IQueueService _queueService;

        [Inject]
        public void Initialize(ICubeRoller cubeSpawner,
            IQueueService queueService)
        {
            _cubeRoller = cubeSpawner;
            _queueService = queueService;
            _cubeSpawnView.RollButton.onClick.AddListener(Roll);
        }

        private void Roll()
        {
            if (!_queueService.CheckTurn())
                return;
            
            _cubeRoller.Roll();
        }
        
        private void OnDestroy() => 
            _cubeSpawnView.RollButton.onClick.RemoveListener(_cubeRoller.Roll);
    }
}