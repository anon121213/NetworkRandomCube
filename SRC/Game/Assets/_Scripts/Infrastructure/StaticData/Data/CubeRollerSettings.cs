using UnityEngine;

namespace _Scripts.Infrastructure.StaticData.Data
{
    [CreateAssetMenu(fileName = "CubeRollerSettings", menuName = "Data/CubeRollerSettings")]
    public class CubeRollerSettings : ScriptableObject
    {
        [field: SerializeField] public float MinThrowForce = 5f;
        [field: SerializeField] public float MaxThrowForce = 15f;
        [field: SerializeField] public float MinTorqueForce = 5f;
        [field: SerializeField] public float MaxTorqueForce = 15f;
    }
}