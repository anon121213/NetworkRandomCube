using System.Reflection;
using _Scripts.Gameplay.CubeComponent;
using _Scripts.Infrastructure.StaticData.Provider;
using _Scripts.Netcore.Data.Attributes;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.RPCSystem;
using _Scripts.Netcore.RPCSystem.ProcessorsData;
using _Scripts.Netcore.Runner;
using UnityEngine;

namespace _Scripts.Gameplay.CubeRoller
{
    public class CubeRoller : NetworkService, ICubeRoller
    {
        private readonly INetworkRunner _networkRunner;
        private readonly ICubeRollerChecker _cubeRollerChecker;
        private readonly MethodInfo _methodInfo = typeof(CubeRoller).GetMethod(nameof(Throw));

        private readonly float _minThrowForce;
        private readonly float _maxThrowForce;
        private readonly float _minTorqueForce;
        private readonly float _maxTorqueForce;

        private Rigidbody rb;

        public CubeRoller(INetworkRunner networkRunner,
            ICubeRollerChecker cubeRollerChecker,
            IStaticDataProvider staticDataProvider)
        {
            _networkRunner = networkRunner;
            _cubeRollerChecker = cubeRollerChecker;
            
            _minThrowForce = staticDataProvider.CubeRollerSettings.MinThrowForce;
            _maxThrowForce = staticDataProvider.CubeRollerSettings.MaxThrowForce;
            _minTorqueForce = staticDataProvider.CubeRollerSettings.MinTorqueForce;
            _maxTorqueForce = staticDataProvider.CubeRollerSettings.MaxTorqueForce;
            
            RPCInvoker.RegisterRPCInstance<CubeRoller>(this);
        }

        public void Initialize(GameObject cube) =>
            rb = cube.GetComponent<Rigidbody>();

        public void Roll()
        {
            if (_networkRunner.IsServer)
            {
                Throw();
                return;
            }

            RPCInvoker.InvokeServiceRPC<CubeRoller>(this, _methodInfo, NetProtocolType.Tcp);
        }

        [ServerRPC]
        public void Throw()
        {
            float throwForce = Random.Range(_minThrowForce, _maxThrowForce);

            rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);

            float torqueX = Random.Range(_minTorqueForce, _maxTorqueForce);
            float torqueY = Random.Range(_minTorqueForce, _maxTorqueForce);
            float torqueZ = Random.Range(_minTorqueForce, _maxTorqueForce);

            rb.AddTorque(new Vector3(torqueX, torqueY, torqueZ), ForceMode.Impulse);

            _cubeRollerChecker.StartRollingCheck();
        }
    }
}