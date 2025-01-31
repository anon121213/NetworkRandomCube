using System;
using System.Threading;
using _Scripts.Infrastructure.StaticData.Provider;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.Runner;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.CubeComponent
{
    public class CubeRollerChecker : NetworkService, ICubeRollerChecker, IDisposable
    {
        private readonly INetworkRunner _networkRunner;
        private readonly CubeFace[] faces; 

        private CancellationTokenSource _cancellationTokenSource;
        private Rigidbody _rb;
        
        private bool _isRolling = true;
        private const float _stopThreshold = 0.03f;

        public event Action<int> OnChangeDiceValue;

        public CubeRollerChecker(IStaticDataProvider staticDataProvider,
            INetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
            faces = staticDataProvider.CubeDiceSettings.faces;
        }

        public void Initialize(GameObject cube) => 
            _rb = cube.GetComponent<Rigidbody>();

        public void StartRollingCheck()
        {
            if (!_networkRunner.IsServer)
                return;
            
            StopRollingCheck();
            _cancellationTokenSource = new CancellationTokenSource();
            CheckRollingLoop(_cancellationTokenSource.Token).Forget();
        }

        public void StopRollingCheck()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTaskVoid CheckRollingLoop(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            
            while (!token.IsCancellationRequested)
            {
                if (_isRolling)
                {
                    if (_rb.velocity.magnitude < _stopThreshold && _rb.angularVelocity.magnitude < _stopThreshold)
                    {
                        _isRolling = false;
                        int result = GetDiceValue();
                        OnChangeDiceValue?.Invoke(result);
                        Debug.Log($"Выпавший номер: {result}");
                        StopRollingCheck();
                    }
                }
                else if (_rb.velocity.magnitude >= _stopThreshold || _rb.angularVelocity.magnitude >= _stopThreshold)
                        _isRolling = true;

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }
        }

        private int GetDiceValue()
        {
            float maxDot = -1f;
            int faceIndex = -1;

            for (int i = 0; i < faces.Length; i++)
            {
                Vector3 worldDirection = _rb.transform.TransformDirection(faces[i].GetDirectionVector());

                float dot = Vector3.Dot(worldDirection, Vector3.up);

                if (!(dot > maxDot))
                    continue;
                
                maxDot = dot;
                faceIndex = i;
            }

            return faces[faceIndex].value;
        }

        public void Dispose() => 
            StopRollingCheck();
    }
}