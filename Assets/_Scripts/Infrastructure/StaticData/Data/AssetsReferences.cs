using _Scripts.Gameplay.CubeComponent;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Infrastructure.StaticData.Data
{
    [CreateAssetMenu(fileName = "AssetsReferences", menuName = "Data/AssetsReferences")]
    public class AssetsReferences: ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject ConnectPanel { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject Cube { get; private set; }
    }
    
    [CreateAssetMenu(fileName = "CubeDiceSettings", menuName = "Data/CubeDiceSettings")]
    public class CubeDiceSettings: ScriptableObject
    {
        [field: SerializeField] public CubeFace[] faces { get; private set; }
    }
}