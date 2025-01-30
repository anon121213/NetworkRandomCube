using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "AllData", menuName = "ScriptableObject/AllData")]
    public class AllData : ScriptableObject
    {
        public AssetsReferences AssetsReferences;
    }

    [CreateAssetMenu(fileName = "AssetsReferences", menuName = "ScriptableObject/AssetsReferences")]
    public class AssetsReferences: ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject ConnectPanel { get; private set; }
    }
    
    public class StaticDataProvider : IStaticDataProvider
    {
        public StaticDataProvider(AllData allData)
        {
            AssetsReferences = allData.AssetsReferences;
        }

        public AssetsReferences AssetsReferences { get; }
    }

    public interface IStaticDataProvider
    {
        AssetsReferences AssetsReferences { get; }
    }
}