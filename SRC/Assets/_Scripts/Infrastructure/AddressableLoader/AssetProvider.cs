using System.Collections.Generic;
using System.Linq;
using _Scripts.Infrastructure.StaticData.Provider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts.Infrastructure.AddressableLoader
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, List<AsyncOperationHandle>> _usedResources = new();

        public async UniTask<T> LoadAsync<T>(string address) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            RegisterForCleanup(address, handle);

            return await handle.Task;
        }

        public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            RegisterForCleanup(assetReference.AssetGUID, handle);

            return await handle.Task;
        }

        public async UniTask<List<T>> LoadAssetsByLabelAsync<T>(string label) where T : class
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            await handle.Task;

            foreach (var asset in handle.Result)
            {
                var key = asset.GetType().Name;
                RegisterForCleanup(key, handle);
            }

            return handle.Result.ToList();
        }

        public void Cleanup()
        {
            foreach (var handle in _usedResources.Values.SelectMany(resourceHandles => resourceHandles))
                Addressables.Release(handle);

            _usedResources.Clear();
        }

        private void RegisterForCleanup<T>(string guid, AsyncOperationHandle<T> handle)
        {
            if (!_usedResources.TryGetValue(guid, out var resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _usedResources[guid] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }
    }

    public interface IAssetProvider
    {
        UniTask<T> LoadAsync<T>(string address) where T : class;
        UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        UniTask<List<T>> LoadAssetsByLabelAsync<T>(string label) where T : class;
        void Cleanup();
    }
}