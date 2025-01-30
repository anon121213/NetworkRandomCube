using _Scripts.Gameplay.CubeComponent;
using UnityEngine;

namespace _Scripts.Infrastructure.StaticData.Data
{
    [CreateAssetMenu(fileName = "CubeDiceSettings", menuName = "Data/CubeRollerSettings")]
    public class CubeRollerSettings : ScriptableObject
    {
        [field: SerializeField] public CubeFace[] faces { get; private set; }

        [field: SerializeField] public float MinThrowForce = 5f;
        [field: SerializeField] public float MaxThrowForce = 15f;
        [field: SerializeField] public float MinTorqueForce = 5f;
        [field: SerializeField] public float MaxTorqueForce = 15f;
    }
}