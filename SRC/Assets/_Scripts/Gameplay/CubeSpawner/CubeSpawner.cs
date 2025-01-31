using _Scripts.Infrastructure.AddressableLoader;
using _Scripts.Infrastructure.StaticData.Provider;
using _Scripts.Netcore.NetworkComponents.RPCComponents;
using _Scripts.Netcore.Spawner;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.CubeSpawner
{
    public class CubeSpawner : NetworkService, ICubeSpawner
    {
        private readonly INetworkSpawner _networkSpawner;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IAssetProvider _assetProvider;

        public CubeSpawner(INetworkSpawner networkSpawner,
            IStaticDataProvider staticDataProvider,
            IAssetProvider assetProvider)
        {
            _networkSpawner = networkSpawner;
            _staticDataProvider = staticDataProvider;
            _assetProvider = assetProvider;
        }
        
        public async UniTask<GameObject> Spawn()
        {
            return await _networkSpawner.Spawn(_staticDataProvider.AssetsReferences.Cube, new Vector3(0, 10, 0));
        }
    }

    public interface ICubeSpawner
    {
        UniTask<GameObject> Spawn();
    }
}