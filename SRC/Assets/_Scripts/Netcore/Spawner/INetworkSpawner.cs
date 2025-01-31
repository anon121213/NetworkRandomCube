using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Netcore.Spawner
{
    public interface INetworkSpawner
    {
        UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Transform transform = null);
        UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Vector3 position, Transform transform = null);
        UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Vector3 position, Quaternion rotation, Transform transform = null);
        UniTask<GameObject> Spawn(AssetReferenceGameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform transform = null);
        void Sync();
    }
}