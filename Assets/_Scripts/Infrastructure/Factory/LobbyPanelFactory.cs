using _Scripts.Infrastructure.AddressableLoader;
using _Scripts.Infrastructure.StaticData;
using _Scripts.Infrastructure.StaticData.Provider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Factory
{
    public class LobbyPanelFactory : ILobbyPanelFactory
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;

        public LobbyPanelFactory(IStaticDataProvider staticDataProvider,
            IAssetProvider assetProvider,
            IObjectResolver objectResolver)
        {
            _staticDataProvider = staticDataProvider;
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }

        public async UniTask<GameObject> CreateLobbyPanel(Transform transform)
        {
             GameObject prefab = await _assetProvider.LoadAsync<GameObject>(_staticDataProvider.AssetsReferences.ConnectPanel);
             return _objectResolver.Instantiate(prefab, transform);
        }
    }

    public interface ILobbyPanelFactory
    {
        UniTask<GameObject> CreateLobbyPanel(Transform transform);
    }
}