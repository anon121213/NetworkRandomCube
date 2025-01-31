using UnityEngine;

namespace _Scripts.Gameplay.CubeRoller
{
    public interface ICubeRoller
    {
        void Initialize(GameObject cube);
        void Roll();
    }
}