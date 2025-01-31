using System;
using System.Reflection;
using _Scripts.Infrastructure.AddressableLoader;
using _Scripts.Netcore.Data.Attributes;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.RPCSystem;
using _Scripts.Netcore.RPCSystem.ProcessorsData;
using _Scripts.Netcore.Runner;
using _Scripts.Netcore.Spawner.ObjectsSyncer;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Netcore.Spawner
{
    public class NetworkSpawner : NetworkService, INetworkSpawner, IDisposable
    {
        private readonly IObjectResolver _resolver;
        private readonly INetworkObjectSyncer _networkObjectSyncer;
        private readonly INetworkRunner _networkRunner;
        private readonly IAssetProvider _assetProvider;
        private readonly MethodInfo _spawnMethodInfo = typeof(NetworkSpawner).GetMethod(nameof(SpawnClientRpc));

        private int _uniqueId = 0;
        
        public NetworkSpawner(IObjectResolver resolver,
            INetworkObjectSyncer networkObjectSyncer,
            INetworkRunner networkRunner,
            IAssetProvider assetProvider)
        {
            _resolver = resolver;
            _networkObjectSyncer = networkObjectSyncer;
            _networkRunner = networkRunner;
            _assetProvider = assetProvider;
            
            _networkRunner.OnPlayerConnected += Sync;
            RPCInvoker.RegisterRPCInstance<NetworkSpawner>(this);
        }

        public async UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Transform transform = null) => 
            await SpawnLocal(prefab, Vector3.zero, Quaternion.identity, Vector3.one, transform);

        public async UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Vector3 position, Transform transform = null) => 
            await SpawnLocal(prefab, position, Quaternion.identity, Vector3.one, transform);

        public async UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Vector3 position, Quaternion rotation, Transform transform = null) => 
            await SpawnLocal(prefab, position, rotation, Vector3.one, transform);

        public async UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform transform = null) => 
            await SpawnLocal(prefab, position, rotation, Vector3.one, transform);

        public void Sync() => 
            _networkObjectSyncer.Sync(this);
        
        private void Sync(int id) => 
            _networkObjectSyncer.Sync(this);

        private async UniTask<GameObject> SpawnLocal(AssetReferenceGameObject reference, Vector3 position, Quaternion rotation, Vector3 scale, Transform transform)
        {
            if (!_networkRunner.IsServer)
                return null;

            var prefab = await _assetProvider.LoadAsync<GameObject>(reference);
            
            if (prefab.GetComponentInChildren<INetworkComponent>() == null)
                return null;
            
            GameObject go = _resolver.Instantiate(prefab, position, rotation, transform);
            go.transform.localScale = scale;
            
            _networkObjectSyncer.AddNetworkObject(reference, _uniqueId, go);

            _uniqueId++;
            
            RPCInvoker.InvokeServiceRPC<NetworkSpawner>(this, _spawnMethodInfo,
                NetProtocolType.Tcp, reference, position, rotation, scale);

            return go;
        }

        [ClientRPC]
        public async void SpawnClientRpc(AssetReferenceGameObject reference, int uniqueId, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (_networkObjectSyncer.CheckSyncObject(reference, uniqueId))
                return;
            
            var prefab = await _assetProvider.LoadAsync<GameObject>(reference);
            
            GameObject networkObj = _resolver.Instantiate(prefab, position, rotation);
            networkObj.transform.localScale = scale;
            
            _networkObjectSyncer.AddNetworkObject(reference, uniqueId, networkObj);
        }

        public void Dispose() => 
            _networkRunner.OnPlayerConnected -= Sync;
    }
}
