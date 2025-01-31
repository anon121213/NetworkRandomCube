using _Scripts.Gameplay.CubeComponent;
using _Scripts.Gameplay.RollQueue;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.CubeRoller
{
    public class CubeRollPresenter : MonoBehaviour
    {
        [SerializeField] private CubeRollView _cubeSpawnView;
        
        private ICubeRoller _cubeRoller;
        private IQueueService _queueService;
        private ICubeRollerChecker _rollerChecker;

        [Inject]
        public void Initialize(ICubeRoller cubeSpawner,
            IQueueService queueService,
            ICubeRollerChecker rollerChecker)
        {
            _cubeRoller = cubeSpawner;
            _queueService = queueService;
            _rollerChecker = rollerChecker;
            _cubeSpawnView.RollButton.onClick.AddListener(Roll);
        }

        private void Roll()
        {
            if (!_queueService.CheckTurn())
                return;

            if (_rollerChecker.IsRolling.Value)
                return;
            
            _cubeRoller.Roll();
        }
        
        private void OnDestroy() => 
            _cubeSpawnView.RollButton.onClick.RemoveListener(_cubeRoller.Roll);
    }
}