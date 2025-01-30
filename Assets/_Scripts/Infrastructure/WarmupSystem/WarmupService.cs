using System.Collections.Generic;
using _Scripts.Infrastructure.AddressableLoader;
using _Scripts.Infrastructure.StaticData;
using _Scripts.Infrastructure.StaticData.Provider;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Infrastructure.WarmupSystem
{
    public class WarmupService : IWarmupService
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IAssetProvider _assetProvider;

        public WarmupService(IStaticDataProvider staticDataProvider,
            IAssetProvider assetProvider)
        {
            _staticDataProvider = staticDataProvider;
            _assetProvider = assetProvider;
        }

        public async UniTask Warmup()
        {
            List<UniTask> tasks = new List<UniTask>
            {
                _assetProvider.InitializeAsset(),
                _assetProvider.LoadAsync<GameObject>(_staticDataProvider.AssetsReferences.ConnectPanel)
            };

            await UniTask.WhenAll(tasks);
        }
    }

    public interface IWarmupService
    {
        UniTask Warmup();
    }
}