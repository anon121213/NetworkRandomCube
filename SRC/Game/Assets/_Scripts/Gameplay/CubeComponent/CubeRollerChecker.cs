﻿using System;
using System.Collections.Generic;
using System.Threading;
using _Scripts.Gameplay.CubeComponent.Data;
using _Scripts.Infrastructure.StaticData.Provider;
using _Scripts.Netcore.NetworkComponents.NetworkVariableComponent;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.Runner;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.CubeComponent
{
    public class CubeRollerChecker : NetworkService, ICubeRollerChecker, IDisposable
    {
        private readonly INetworkRunner _networkRunner;
        private readonly CubeFace[] _faces; 

        private CancellationTokenSource _cancellationTokenSource;
        private Rigidbody _rb;

        public NetworkVariable<bool> IsRolling { get; set; } = new("IsRolling", false);
        private const float _stopThreshold = 0.03f;

        public event Action<int> OnChangeDiceValue;

        public CubeRollerChecker(IStaticDataProvider staticDataProvider,
            INetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
            _faces = staticDataProvider.CubeDiceSettings.faces;
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
                if (IsRolling.Value)
                {
                    if (_rb.velocity.magnitude < _stopThreshold && _rb.angularVelocity.magnitude < _stopThreshold)
                    {
                        IsRolling.Value = false;
                        int result = GetDiceValue();
                        OnChangeDiceValue?.Invoke(result);
                        Debug.Log($"Выпавший номер: {result}");
                        StopRollingCheck();
                    }
                }
                else if (_rb.velocity.magnitude >= _stopThreshold || _rb.angularVelocity.magnitude >= _stopThreshold)
                        IsRolling.Value = true;

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }
        }

        private int GetDiceValue()
        {
            float maxDot = -1f;
            int faceIndex = -1;

            for (int i = 0; i < _faces.Length; i++)
            {
                Vector3 worldDirection = _rb.transform.TransformDirection(_faces[i].GetDirectionVector());

                float dot = Vector3.Dot(worldDirection, Vector3.up);

                if (!(dot > maxDot))
                    continue;
                
                maxDot = dot;
                faceIndex = i;
            }

            return _faces[faceIndex].value;
        }

        public void Dispose() => 
            StopRollingCheck();
    }
}