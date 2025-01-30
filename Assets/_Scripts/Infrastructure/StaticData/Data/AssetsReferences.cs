using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Infrastructure.StaticData.Data
{
    [CreateAssetMenu(fileName = "AssetsReferences", menuName = "ScriptableObject/AssetsReferences")]
    public class AssetsReferences: ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject ConnectPanel { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject Cube { get; private set; }
    }
}