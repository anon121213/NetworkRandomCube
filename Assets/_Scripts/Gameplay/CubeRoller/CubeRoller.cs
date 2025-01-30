using System.Reflection;
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
        private readonly MethodInfo _methodInfo = typeof(CubeRoller).GetMethod(nameof(Throw));
        
        private const float _minThrowForce = 5f; 
        private const float _maxThrowForce = 15f; 
        private const float _minTorqueForce = 5f; 
        private const float _maxTorqueForce = 15f;

        private Rigidbody rb;

        public CubeRoller(INetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
            RPCInvoker.RegisterRPCInstance<CubeRoller>(this);
        }

        public void Initialize(GameObject cube) => 
            rb = cube.GetComponent<Rigidbody>();

        public void ThrowDice()
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
        }
    }

    public interface ICubeRoller
    {
        void Initialize(GameObject cube);
        void ThrowDice();
    }
}