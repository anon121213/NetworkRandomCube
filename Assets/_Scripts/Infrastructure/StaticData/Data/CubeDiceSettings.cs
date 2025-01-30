using _Scripts.Gameplay.CubeComponent;
using UnityEngine;

namespace _Scripts.Infrastructure.StaticData.Data
{
    [CreateAssetMenu(fileName = "CubeDiceSettings", menuName = "Data/CubeDiceSettings")]
    public class CubeDiceSettings : ScriptableObject
    {
        [field: SerializeField] public CubeFace[] faces { get; private set; }
    }
}