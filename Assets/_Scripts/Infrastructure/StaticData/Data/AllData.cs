using UnityEngine;

namespace _Scripts.Infrastructure.StaticData.Data
{
    [CreateAssetMenu(fileName = "AllData", menuName = "ScriptableObject/AllData")]
    public class AllData : ScriptableObject
    {
        public AssetsReferences AssetsReferences;
        public CubeDiceSettings CubeDiceSettings;
    }
}